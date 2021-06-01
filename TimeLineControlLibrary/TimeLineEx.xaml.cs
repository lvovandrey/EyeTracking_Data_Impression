using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeLineControlLibrary
{
    /// <summary>
    /// Логика взаимодействия для TimeLineEx.xaml
    /// </summary>
    public partial class TimeLineEx : UserControl, INotifyPropertyChanged
    {

        public TimeLineEx()
        {
            InitializeComponent();


            T_tenSec.T_full = FullTime;
            T_tenSec.T_el = TimeSpan.FromSeconds(10);
            T_tenSec.ChangeDashesHeight(10);

            OnFullTimeChanged += TimeLineEx_OnFullTimeChanged;
            OnBarsChanged += TimeLineEx_OnBarsChanged;

        }

        private void THIS_Loaded(object sender, RoutedEventArgs e)
        {
            GridMain.Width = DefaultGridMainWidth;
        }





        #region DependencyProperty POS - позиция на видео для биндинга к видеоплееру
        public static readonly DependencyProperty POSProperty = DependencyProperty.Register("POS",
         typeof(double), typeof(TimeLineEx),
         new FrameworkPropertyMetadata(new PropertyChangedCallback(POSPropertyChangedCallback)));

        public double POS
        {
            get { return (double)GetValue(POSProperty); }
            set { SetValue(POSProperty, value); }
        }


        public event PropertyChanged OnPOSChanged;


        static void POSPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLineEx)d).OnPOSChanged != null)
                ((TimeLineEx)d).OnPOSChanged(d, e);
        }



        #endregion


        #region intervalsOperations

        //DependencyProperty FullTime  - чтобы можно было подписаться на него
        public TimeSpan FullTime
        {
            get { return (TimeSpan)GetValue(FullTimeProperty); }
            set
            {
                SetValue(FullTimeProperty, value);
                RefreshDashes();
                OnPropertyChanged("FullTime");
            }
        }

        public static readonly DependencyProperty FullTimeProperty =
            DependencyProperty.Register("FullTime", typeof(TimeSpan), typeof(TimeLineEx), new PropertyMetadata(TimeSpan.FromSeconds(10), new PropertyChangedCallback(FullTimePropertyChangedCallback)));

        public event PropertyChanged OnFullTimeChanged;

        private static void FullTimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLineEx)d).OnFullTimeChanged != null)
                ((TimeLineEx)d).OnFullTimeChanged(d, e);
        }

        private void TimeLineEx_OnFullTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RefreshDashes();
        }

        private double zoomKoef = 1.3;
        public double ZoomKoef
        {
            get { return zoomKoef; }
            set { if (value <= 1) throw new Exception("Try set to zoom koefficient incorrect value"); zoomKoef = value; }
        }


        private double OffsetViewport { get { return ScrollViewerMain.HorizontalOffset; } }
        private double WidthViewport { get { return GridViewport.ActualWidth; } }
        private double WidthGridMain { get { return GridMain.ActualWidth; } }
        private TimeSpan TimeIntervalViewport { get { return TimeSpan.FromSeconds(FullTime.TotalSeconds * WidthViewport / WidthGridMain); } }
        private TimeSpan TimeBeginViewport { get { return TimeSpan.FromSeconds(FullTime.TotalSeconds * OffsetViewport / WidthGridMain); } }
        private TimeSpan TimeEndViewport { get { return TimeBeginViewport + TimeIntervalViewport; } }
        private List<Bar> BarsInViewport { get { return GetBarsInViewport(); } }
        private TimeSpan ViewportStockRatioTime { get { return TimeIntervalViewport; } }
        private double ViewportScalePxInSecond { get { return WidthViewport / TimeIntervalViewport.TotalSeconds; } }

        private TimeSpan DefaultViewportTimeInterval
        {
            get
            {
                if (FullTime > TimeSpan.FromMinutes(1)) return TimeSpan.FromMinutes(1);
                else return FullTime;
            }
        }
        private double DefaultGridMainWidth
        {
            get { return GridViewport.ActualWidth * (FullTime.TotalSeconds / DefaultViewportTimeInterval.TotalSeconds); }
        }



        private List<Bar> GetBarsInViewport()
        {
            var t1 = TimeBeginViewport - ViewportStockRatioTime; if (t1 < TimeSpan.Zero) t1 = TimeSpan.Zero;
            var t2 = TimeEndViewport + ViewportStockRatioTime; if (t2 > FullTime) t2 = FullTime;

            var barsInViewport = Bars.Where(i =>
            {
                var a = i.TimeBegin;
                var b = i.TimeEnd;
                if ((b > t1 && b < t2) || (a > t1 && a < t2) || (a < t1 && b > t2)) //если bar пересекает одну из границ вьюпорта или заполняет его весь условие выполнится
                { return true; }
                return false;
            }
            ).ToList();
            return barsInViewport;
        }


        //DependencyProperty Bars  - чтобы можно было подписаться на него
        public ObservableCollection<Bar> Bars
        {
            get { return (ObservableCollection<Bar>)GetValue(BarsProperty); }
            set
            {
                SetValue(BarsProperty, value);
                RefreshDashes();
                OnPropertyChanged("Bars");
            }
        }

        public static readonly DependencyProperty BarsProperty =
            DependencyProperty.Register("Bars", typeof(ObservableCollection<Bar>), typeof(TimeLineEx), new PropertyMetadata(new ObservableCollection<Bar>(), new PropertyChangedCallback(BarsPropertyChangedCallback)));

        public event PropertyChanged OnBarsChanged;

        private static void BarsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLineEx)d).OnBarsChanged != null)
                ((TimeLineEx)d).OnBarsChanged(d, e);
        }

        private void TimeLineEx_OnBarsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RefreshDashes();

            RefreshVisibleBars();
            ScaleDashes();
        }

        void RefreshVisibleBars()
        {
            BarsArea.ClearBars();

            foreach (var bar in BarsInViewport)
            {
                BarsArea.AddBar(bar);
            }
        }


        void HideAllDashes()
        {
            T_tenSec.TimeLabelVisibility = Visibility.Hidden;
            T_tenSec.Visibility = Visibility.Hidden;
        }

        void ScaleDashes()
        {
            HideAllDashes();
            var Sc = ViewportScalePxInSecond;
            if (Sc > 5)
            {
                T_tenSec.TimeLabelVisibility = Visibility.Visible;
            }
            if (Sc > 0.5)
            {
                T_tenSec.Visibility = Visibility.Visible;
            }
        }

        void RefreshDashes()
        {
            T_tenSec.T_full = FullTime;
            T_tenSec.T_el = TimeSpan.FromSeconds(10);
            T_tenSec.ChangeDashesHeight(14);
            T_tenSec.ChangeDashesWidth(1.5);
            T_tenSec.Visibility = Visibility.Visible;
            T_tenSec.TimeLabelVisibility = Visibility.Visible;
        }






        #endregion

       
        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion


        private void THIS_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

            // Console.WriteLine("offset={0} x={1} horOffset={2}", offset, Mouse.GetPosition(GridMain).X, ScrollViewerMain.HorizontalOffset);
            if (e.Delta > 0)
            {
                GridMain.Width = GridMain.ActualWidth * ZoomKoef;

                double offset = Mouse.GetPosition(GridMain).X * (ZoomKoef - 1);
                ScrollViewerMain.ScrollToHorizontalOffset(ScrollViewerMain.HorizontalOffset + offset);
            }
            else
            {
                GridMain.Width = GridMain.ActualWidth / ZoomKoef;

                double offset = Mouse.GetPosition(GridMain).X * (1 - (1 / ZoomKoef));
                ScrollViewerMain.ScrollToHorizontalOffset(ScrollViewerMain.HorizontalOffset - offset);
            }

            RefreshVisibleBars();
            ScaleDashes();
        }


        private void ScrollViewerMain_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            RefreshVisibleBars();
            ScaleDashes();
        }


    }
}
}
