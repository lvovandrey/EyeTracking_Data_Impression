﻿using System;
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
    public delegate void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);

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

        bool alreadyLoaded = false;
        private void THIS_Loaded(object sender, RoutedEventArgs e)
        {
            if (alreadyLoaded) return;
            SetupTimeDashesAreas();
            GridMain.Width = DefaultGridMainWidth;
            VirtualizationBuffer = new VirtualizationBuffer(this, 3, null);
            alreadyLoaded = true;
            ToolsTimer.Delay(() => //TODO:Костыль
            {
                VirtualizationDrawRun(true);
                RefreshVisibleBars();
                ScaleDashes();
            }, TimeSpan.FromSeconds(1));
        }


        #region DashOperations

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
            BarsArea.ClearBars();
            foreach (var bar in BarsInViewport)
            {
                BarsArea.AddBar(bar);
            }
        }

        void ClearAllDashes()
        {
            foreach (var area in TimeDashesAreas)
            {
                area.Value.ClearAllDashes();
            }
        }
        void HideAllDashes()
        {
            foreach (var area in TimeDashesAreas)
            {
                area.Value.TimeLabelVisibility = Visibility.Hidden;
                area.Value.Visibility = Visibility.Hidden;
            }
        }

        void ScaleDashes()
        {
            HideAllDashes();
            var Sc = ViewportScalePxInSecond;
            foreach (var area in TimeDashesAreas)
            {
                if (Sc > area.Value.TimeLabelVisibilityResolution)
                    area.Value.TimeLabelVisibility = Visibility.Visible;
                if (Sc > area.Value.VisibilityResolution)
                    area.Value.Visibility = Visibility.Visible;
            }

        }

        internal Dictionary<string, TimeDashesArea> TimeDashesAreas = new Dictionary<string, TimeDashesArea>();
        void SetupTimeDashesAreas()
        {

            T_100msec.T_el = TimeSpan.FromMilliseconds(100);
            T_100msec.DashWidth = 1;
            T_100msec.DashHeight = 5;
            T_100msec.VisibilityResolution = 100;
            T_100msec.TimeLabelVisibilityResolution = 500;

            T_1Sec.T_el = TimeSpan.FromSeconds(1);
            T_1Sec.DashWidth = 1;
            T_1Sec.DashHeight = 10;
            T_1Sec.VisibilityResolution = 10;
            T_1Sec.TimeLabelVisibilityResolution = 50;

            T_10Sec.T_el = TimeSpan.FromSeconds(10);
            T_10Sec.DashWidth = 1.5;
            T_10Sec.DashHeight = 13;
            T_10Sec.VisibilityResolution = 1;
            T_10Sec.TimeLabelVisibilityResolution = 5;

            T_1Min.T_el = TimeSpan.FromMinutes(1);
            T_1Min.DashWidth = 2;
            T_1Min.DashHeight = 16;
            T_1Min.VisibilityResolution = 0.5;
            T_1Min.TimeLabelVisibilityResolution = 1;

            T_10Min.T_el = TimeSpan.FromMinutes(10);
            T_10Min.DashWidth = 2;
            T_10Min.DashHeight = 20;
            T_10Min.VisibilityResolution = 0.05;
            T_10Min.TimeLabelVisibilityResolution = 0.25;

            T_1Hour.T_el = TimeSpan.FromHours(1);
            T_1Hour.DashWidth = 3;
            T_1Hour.DashHeight = 25;
            T_1Hour.VisibilityResolution = 0.01;
            T_1Hour.TimeLabelVisibilityResolution = 0.05;

            TimeDashesAreas.Add(T_100msec.Name, T_100msec);
            TimeDashesAreas.Add(T_1Sec.Name, T_1Sec);
            TimeDashesAreas.Add(T_10Sec.Name, T_10Sec);
            TimeDashesAreas.Add(T_1Min.Name, T_1Min);
            TimeDashesAreas.Add(T_10Min.Name, T_10Min);
            TimeDashesAreas.Add(T_1Hour.Name, T_1Hour);

            foreach (var area in TimeDashesAreas)
            {
                area.Value.T_full = FullTime;
                area.Value.TimeLineEx = this;
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
            foreach (var a in TimeDashesAreas)
            {
                var area = a.Value;
                if (area.Visibility != Visibility.Visible) continue;
                foreach (var interval in VirtualizationBuffer.UnusedIntervals)
                    area.EraseDashesInInterval(interval.Begin, interval.End);
                foreach (var interval in VirtualizationBuffer.NewIntervals)
                    area.DrawAllDashesInInterval(interval.Begin, interval.End);
                if (OldVirtualizationBuffer == null || forceRedraw)//если буфера нет - значит это первая прорисовка и нужно отрисовать вообще все
                    area.DrawAllDashesInInterval(VirtualizationBuffer.Interval.Begin, VirtualizationBuffer.Interval.End);
            }
            OldVirtualizationBuffer = VirtualizationBuffer;
                       
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
            if (e.Delta > 0)
            {
                RealCurrentWidth = GridMain.ActualWidth * ZoomKoef;
                GridMain.Width = GridMain.ActualWidth * ZoomKoef;

                double offset = Mouse.GetPosition(GridMain).X * (ZoomKoef - 1);
                ScrollViewerMain.ScrollToHorizontalOffset(ScrollViewerMain.HorizontalOffset + offset);
            }
            else
            {
                RealCurrentWidth = GridMain.ActualWidth / ZoomKoef;
                GridMain.Width = GridMain.ActualWidth / ZoomKoef;

                double offset = Mouse.GetPosition(GridMain).X * (1 - (1 / ZoomKoef));
                ScrollViewerMain.ScrollToHorizontalOffset(ScrollViewerMain.HorizontalOffset - offset);
            }

            RefreshVisibleBars();
            ScaleDashes();
            ClearAllDashes();
            VirtualizationDrawRun(true);
            zoomflag = true;

            Console.WriteLine(ViewportScalePxInSecond);
        }

        public double RealCurrentWidth;

        private bool zoomflag = false;//ебаное говнище!!!!! fuuck

        private void ScrollViewerMain_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            RealCurrentWidth = GridMain.ActualWidth;
            if (zoomflag) { zoomflag = false; return; }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            VirtualizationDrawRun(false);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("Dashes " + ts.TotalMilliseconds);
            stopWatch.Restart();

            RefreshVisibleBars();

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("Bars " + ts.TotalMilliseconds);
            stopWatch.Restart();

            ScaleDashes();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            Console.WriteLine("ScaleDashes " + ts.TotalMilliseconds);

        }


    }
}

