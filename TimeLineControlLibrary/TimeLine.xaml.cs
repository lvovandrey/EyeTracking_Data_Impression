
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace TimeLineControlLibrary
{
    public delegate void ZoomDelegate(double MousePositionX, double ZoomKoef);

    /// <summary>
    /// Логика взаимодействия для TimeLine.xaml
    /// </summary>
    public partial class TimeLine : UserControl, INotifyPropertyChanged
    {

        public event ZoomDelegate Zoom;
        public double ZoomKoef = 1.3;

        public TimeLine()
        {
            InitializeComponent();

            T1.T_full = FullTime;
            T1.T_el = TimeSpan.FromSeconds(60);
            T1.ChangeDashesHeight(20);
            T1.ChangeDashesWidth(2);
            T_tenSec.T_full = FullTime;
            T_tenSec.T_el = TimeSpan.FromSeconds(10);
            T_tenSec.ChangeDashesHeight(10);

            Cursor1.Container = this;

            Binding binding = new Binding();
            binding.ElementName = "Cursor1"; // элемент-источник
            binding.Path = new PropertyPath("CRPosition"); // свойство элемента-источника
            binding.Mode = BindingMode.TwoWay;
            this.SetBinding(TimeLine.POSProperty, binding); // установка привязки для элемента-приемника

            OnPOSChanged += TimeLine_OnPOSChanged;
            OnFullTimeChanged += TimeLine_OnFullTimeChanged;
            OnBarsChanged += TimeLine_OnBarsChanged;
            Cursor1.OnCRPChanged += Cursor1_OnCRPChanged;
            Cursor1.OnStartDrag += Cursor1_OnStartDrag;
            Cursor1.OnEndDrag += Cursor1_OnEndDrag;

            SizeChanged += (d, e) => { RefreshCusorPosition(); };
        }




        #region Перемещения курсора
        bool wasplayed = false;
        private void Cursor1_OnStartDrag()
        {

        }
        private void Cursor1_OnEndDrag()
        {

        }




        bool PosSelf = false;
        private void Cursor1_OnCRPChanged()
        {
            PosSelf = true;
            POS = Cursor1.CRPosition * 1000;
        }

        public void TimeLine_OnPOSChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RefreshCusorPosition();
        }

        public void RefreshCusorPosition()
        {
            if (PosSelf) { PosSelf = false; return; }
            if (POS > -0.1) { Cursor1.CRPosition = POS / 1000; Cursor1.RefreshPosition(); }

        }
        #endregion



        #region DependencyProperty POS - позиция на видео для биндинга к видеоплееру
        public static readonly DependencyProperty POSProperty = DependencyProperty.Register("POS",
         typeof(double), typeof(TimeLine),
         new FrameworkPropertyMetadata(new PropertyChangedCallback(POSPropertyChangedCallback)));

        public double POS
        {
            get { return (double)GetValue(POSProperty); }
            set { SetValue(POSProperty, value); }
        }


        public event PropertyChanged OnPOSChanged;


        static void POSPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLine)d).OnPOSChanged != null)
                ((TimeLine)d).OnPOSChanged(d, e);
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
            DependencyProperty.Register("FullTime", typeof(TimeSpan), typeof(TimeLine), new PropertyMetadata(TimeSpan.FromSeconds(10), new PropertyChangedCallback(FullTimePropertyChangedCallback)));

        public event PropertyChanged OnFullTimeChanged;

        private static void FullTimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLine)d).OnFullTimeChanged != null)
                ((TimeLine)d).OnFullTimeChanged(d, e);
        }

        private void TimeLine_OnFullTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RefreshDashes();
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
            DependencyProperty.Register("Bars", typeof(ObservableCollection<Bar>), typeof(TimeLine), new PropertyMetadata(new ObservableCollection<Bar>(), new PropertyChangedCallback(BarsPropertyChangedCallback)));

        public event PropertyChanged OnBarsChanged;

        private static void BarsPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeLine)d).OnBarsChanged != null)
                ((TimeLine)d).OnBarsChanged(d, e);
        }

        private void TimeLine_OnBarsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RefreshDashes();

            BARS.ClearBars();
            //Bars.Clear();
            //Random random = new Random();

            foreach (var bar in Bars)
            {
                BARS.AddBar(bar);
            }

        }




        void RefreshDashes()
        {

            T1.T_full = FullTime;
            T_tenSec.T_full = FullTime;
            T_Sec.T_full = FullTime;
            T10.T_full = FullTime;
            T_Sec_tenth_part.T_full = FullTime;



            T1.T_el = TimeSpan.FromSeconds(60);
            T_tenSec.T_el = TimeSpan.FromSeconds(10);
            T_Sec.T_el = TimeSpan.FromSeconds(1);
            T10.T_el = TimeSpan.FromSeconds(600);
            T_Sec_tenth_part.T_el = TimeSpan.FromMilliseconds(100);

            int N = (int)Math.Round((FullTime.TotalSeconds / T1.T_el.TotalSeconds)) + 2;
            T1.ClearDashes();
            T1.FillDashes(N);

            N = (int)Math.Round((FullTime.TotalSeconds / T_tenSec.T_el.TotalSeconds)) + 2;
            T_tenSec.ClearDashes();
            T_tenSec.FillDashes(N);

            if (FullTime.TotalSeconds < 3600)
            {
                N = (int)Math.Round((FullTime.TotalSeconds / T_Sec.T_el.TotalSeconds)) + 2;
                T_Sec.ClearDashes();
                T_Sec.FillDashes(N);
            }

            if (FullTime.TotalSeconds < 60)
            {
                N = (int)Math.Round((FullTime.TotalSeconds / T_Sec_tenth_part.T_el.TotalSeconds)) + 2;
                T_Sec_tenth_part.ClearDashes();
                T_Sec_tenth_part.FillDashes(N);
            }


            N = (int)Math.Round((FullTime.TotalSeconds / T10.T_el.TotalSeconds)) + 2;
            T10.ClearDashes();
            T10.FillDashes(N);


            T1.ChangeDashesHeight(20);
            T1.ChangeDashesWidth(1);

            T_tenSec.ChangeDashesHeight(14);

            T_Sec.ChangeDashesHeight(10);

            T_Sec_tenth_part.ChangeDashesHeight(5);

            T10.ChangeDashesHeight(26);
            T10.ChangeDashesWidth(2);

            T1.Visibility = Visibility.Visible;
            T_tenSec.Visibility = Visibility.Visible;
            T_Sec.Visibility = Visibility.Visible;
            T10.Visibility = Visibility.Visible;
            T_Sec_tenth_part.Visibility = Visibility.Hidden;


            T1.TimeLabelVisibility = Visibility.Hidden;
            T_tenSec.TimeLabelVisibility = Visibility.Visible;
            T_Sec.TimeLabelVisibility = Visibility.Visible;
            T10.TimeLabelVisibility = Visibility.Hidden;
            T_Sec_tenth_part.TimeLabelVisibility = Visibility.Hidden;

            if (FullTime < TimeSpan.FromMinutes(0.1))
            {
                T1.TimeLabelVisibility = Visibility.Hidden;
                T_tenSec.TimeLabelVisibility = Visibility.Visible;
                T_Sec.TimeLabelVisibility = Visibility.Visible;
                T10.TimeLabelVisibility = Visibility.Hidden;
                T_Sec_tenth_part.Visibility = Visibility.Visible;
                T_Sec_tenth_part.TimeLabelVisibility = Visibility.Visible;
            }
            else if (FullTime < TimeSpan.FromMinutes(0.5))
            {
                T1.TimeLabelVisibility = Visibility.Hidden;
                T_tenSec.TimeLabelVisibility = Visibility.Visible;
                T_Sec.TimeLabelVisibility = Visibility.Visible;
                T10.TimeLabelVisibility = Visibility.Hidden;
            }
            else if (FullTime < TimeSpan.FromMinutes(30))
            {
                T1.TimeLabelVisibility = Visibility.Visible;
                T_tenSec.TimeLabelVisibility = Visibility.Hidden;
                T_Sec.TimeLabelVisibility = Visibility.Hidden;
                T10.TimeLabelVisibility = Visibility.Visible;
                T_Sec.Visibility = Visibility.Hidden;
            }
            else if (FullTime >= TimeSpan.FromMinutes(30))
            {
                T1.TimeLabelVisibility = Visibility.Hidden;
                T_tenSec.TimeLabelVisibility = Visibility.Hidden;
                T_Sec.TimeLabelVisibility = Visibility.Hidden;
                T10.TimeLabelVisibility = Visibility.Visible;
                T_tenSec.Visibility = Visibility.Hidden;
                T_Sec.Visibility = Visibility.Hidden;
            }

            T_Sec_tenth_part.HideRepeatedDashes(T_Sec.Dashes);
            T_Sec.HideRepeatedDashes(T_tenSec.Dashes);
            T_tenSec.HideRepeatedDashes(T1.Dashes);
            T1.HideRepeatedDashes(T10.Dashes);

        }






        #endregion

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //Перемещаем курсор в точку клика на таймлайне
            Cursor1.SetPosition(0, e);
        }

        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void THIS_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }



        private void THIS_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta>0)
            {
                THIS.Width = THIS.ActualWidth * ZoomKoef;
            }
            else
            {
                THIS.Width = THIS.ActualWidth / ZoomKoef;
            }

            Zoom?.Invoke(Mouse.GetPosition(THIS).X, ZoomKoef);
        }
    }
}

