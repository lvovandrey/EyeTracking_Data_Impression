using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

            OnFullTimeChanged += TimeLineEx_OnFullTimeChanged;
            OnBarsChanged += TimeLineEx_OnBarsChanged;

        }

        private void THIS_Loaded(object sender, RoutedEventArgs e)
        {
            GridMain.Width = DefaultGridMainWidth;
            VirtualizationBuffer = new VirtualizationBuffer(this, 1, null);
            RefreshDashes();
        }





       


        #region intervalsOperations

        //DependencyProperty FullTime  - чтобы можно было подписаться на него
        public TimeSpan FullTime
        {
            get { return (TimeSpan)GetValue(FullTimeProperty); }
            set
            {
                SetValue(FullTimeProperty, value);
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
        }

        private double zoomKoef = 1.3;
        public double ZoomKoef
        {
            get { return zoomKoef; }
            set { if (value <= 1) throw new Exception("Try set to zoom koefficient incorrect value"); zoomKoef = value; }
        }


        public double OffsetViewport { get { return ScrollViewerMain.HorizontalOffset; } }
        public double WidthViewport { get { return GridViewport.ActualWidth; } }
        public double WidthGridMain { get { return GridMain.ActualWidth; } }
        public TimeSpan TimeIntervalViewport { get { return TimeSpan.FromSeconds(FullTime.TotalSeconds * WidthViewport / WidthGridMain); } }
        public TimeSpan TimeBeginViewport { get { return TimeSpan.FromSeconds(FullTime.TotalSeconds * OffsetViewport / WidthGridMain); } }
        public TimeSpan TimeEndViewport { get { return TimeBeginViewport + TimeIntervalViewport; } }
        public List<Bar> BarsInViewport { get { return GetBarsInViewport(); } }
        public TimeSpan ViewportStockRatioTime { get { return TimeIntervalViewport; } }
        public double ViewportScalePxInSecond { get { return WidthViewport / TimeIntervalViewport.TotalSeconds; } }

        public TimeSpan DefaultViewportTimeInterval
        {
            get
            {
                if (FullTime > TimeSpan.FromMinutes(1)) return TimeSpan.FromMinutes(1);
                else return FullTime;
            }
        }
        public double DefaultGridMainWidth
        {
            get { return GridViewport.ActualWidth * (FullTime.TotalSeconds / DefaultViewportTimeInterval.TotalSeconds); }
        }



        public List<Bar> GetBarsInViewport()
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
            //RefreshDashes();
            //RefreshVisibleBars();
            //ScaleDashes();
        }

        void RefreshVisibleBars()
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BarsArea.ClearBars();
            foreach (var bar in BarsInViewport)
            {
                BarsArea.AddBar(bar);
            }

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("Bars   time {0}ms    fulltime {1} sec",
                ts.TotalMilliseconds, TimeIntervalViewport.TotalSeconds);
            Console.WriteLine("RunTime " + elapsedTime);



        }


        void HideAllDashes()
        {
            T_Sec.TimeLabelVisibility = Visibility.Hidden;
            T_Sec.Visibility = Visibility.Hidden;
        }

        void ScaleDashes()
        {
         //   HideAllDashes();
          //  var Sc = ViewportScalePxInSecond;
          //  if (Sc > 50)
            {
                T_Sec.TimeLabelVisibility = Visibility.Visible;
            }
           // if (Sc > 5)
            {
                T_Sec.Visibility = Visibility.Visible;
            }
        }

        VirtualizationBuffer VirtualizationBuffer;
        VirtualizationBuffer OldVirtualizationBuffer;

        void VirtualizationDrawRun(bool forceRedraw)
        {
            VirtualizationBuffer = new VirtualizationBuffer(this, 3, OldVirtualizationBuffer);

            VirtualizationBuffer.ChangeSizeVirtualizerBuffer();
            VirtualizationBuffer.FindUnusedIntervals();
            VirtualizationBuffer.FindNewIntervals();
            foreach (var interval in VirtualizationBuffer.UnusedIntervals)
                T_Sec.EraseDashesInInterval(interval.Begin, interval.End);
            foreach (var interval in VirtualizationBuffer.NewIntervals)
                T_Sec.DrawAllDashesInInterval(interval.Begin, interval.End);
            //if(OldVirtualizationBuffer==null || forceRedraw)//если буфера нет - значит это первая прорисовка и нужно отрисовать вообще все
            T_Sec.DrawAllDashesInInterval(VirtualizationBuffer.Interval.Begin, VirtualizationBuffer.Interval.End);


            OldVirtualizationBuffer = VirtualizationBuffer;
        }

        void RefreshDashes()
        {
            if (T_Sec.Visibility != Visibility.Visible) return;
           
            T_Sec.T_full = FullTime;
            T_Sec.T_el = TimeSpan.FromSeconds(1);
            T_Sec.DashWidth = 2;
            T_Sec.DashHeight = 22;
            //T_Sec.ChangeDashesHeight(14);
            //T_Sec.ChangeDashesWidth(1.5);
            T_Sec.Visibility = Visibility.Visible;
            T_Sec.TimeLabelVisibility = Visibility.Visible;
        }






        #endregion

        #region Buffer for virtualization



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

            //RefreshDashes();
            RefreshVisibleBars();
            // ScaleDashes();
                       

            T_Sec.ClearAllDashes();
            VirtualizationBuffer = new VirtualizationBuffer(this, 3, OldVirtualizationBuffer);

            VirtualizationBuffer.ChangeSizeVirtualizerBuffer();
            T_Sec.DrawAllDashesInInterval(VirtualizationBuffer.Interval.Begin, VirtualizationBuffer.Interval.End);

            OldVirtualizationBuffer = VirtualizationBuffer;
            Console.WriteLine("zoom");
            zoomflag = true;
        }

        private bool zoomflag = false;//ебаное говнище!!!!! fuuck

        private void ScrollViewerMain_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (zoomflag) { zoomflag = false; return; }
          //  RefreshDashes();
            VirtualizationDrawRun(false);
            RefreshVisibleBars();
      //      ScaleDashes();
            Console.WriteLine("scroll");
        }


    }
}

