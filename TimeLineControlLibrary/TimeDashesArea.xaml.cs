﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeLineControlLibrary
{
    /// <summary>
    /// Interaction logic for TimeDashesArea.xaml
    /// </summary>
    public partial class TimeDashesArea : UserControl
    {
        public TimeDashesArea()
        {
            InitializeComponent();
            Dashes = new ObservableCollection<Dash>();
        }


        public TimeSpan Interval { get; set; }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position",
          typeof(double), typeof(TimeDashesArea),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(PositionPropertyChangedCallback)));

        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration",
          typeof(TimeSpan), typeof(TimeDashesArea),
          new FrameworkPropertyMetadata(new PropertyChangedCallback(DurationPropertyChangedCallback)));

        public static readonly DependencyProperty CurTimeProperty = DependencyProperty.Register("CurTime",
            typeof(TimeSpan), typeof(TimeDashesArea),
            new FrameworkPropertyMetadata(new PropertyChangedCallback(CurTimePropertyChangedCallback)));

        public static readonly DependencyProperty TimeLabelVisibilityProperty = DependencyProperty.Register("TimeLabelVisibility",
           typeof(Visibility), typeof(TimeDashesArea),
           new FrameworkPropertyMetadata(new PropertyChangedCallback(TimeLabelVisibilityPropertyChangedCallback)));

        public double Position { get { return (double)GetValue(PositionProperty); } set { SetValue(PositionProperty, value); } }
        public TimeSpan Duration { get { return (TimeSpan)GetValue(DurationProperty); } set { SetValue(DurationProperty, value); } }
        public TimeSpan CurTime { get { return (TimeSpan)GetValue(CurTimeProperty); } set { SetValue(CurTimeProperty, value); } }
        public Visibility TimeLabelVisibility { get { return (Visibility)GetValue(TimeLabelVisibilityProperty); } set { SetValue(TimeLabelVisibilityProperty, value); } }

        public event PropertyChanged OnPositionChanged;
        public event PropertyChanged OnDurationChanged;
        public event PropertyChanged OnCurTimeChanged;
        public event PropertyChanged OnTimeLabelVisibilityChanged;

        public ObservableCollection<Dash> Dashes;


        static void PositionPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeDashesArea)d).OnPositionChanged != null)
                ((TimeDashesArea)d).OnPositionChanged(d, e);
        }

        static void DurationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeDashesArea)d).OnDurationChanged != null)
                ((TimeDashesArea)d).OnDurationChanged(d, e);
        }

        static void CurTimePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeDashesArea)d).OnCurTimeChanged != null)
                ((TimeDashesArea)d).OnCurTimeChanged(d, e);
        }

        static void TimeLabelVisibilityPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TimeDashesArea)d).OnTimeLabelVisibilityChanged != null)
                ((TimeDashesArea)d).OnTimeLabelVisibilityChanged(d, e);
        }



        private void CalcDashesPosition()
        {
            int NDashes;
            NDashes = (int)Duration.TotalMinutes;
            Dashes = new ObservableCollection<Dash>();
            double width = this.ActualWidth;
            double dashIntervals = width / Duration.TotalMinutes;
            double curDashLeftCoord = 0;
            for (int i = 1; i <= NDashes; i++)
            {
                Dash dash = new Dash();
                dash.Margin = new Thickness(curDashLeftCoord, 0, 0, 0); //TODO: где-то тут механизм расчета положения дашей - его нужно поменять в этом элементе
                Dashes.Add(dash);
                curDashLeftCoord += dashIntervals;
            }
        }
        public void UpdateDashes()
        {
            if (Dashes.Count > 0)
                for (int i = 0; i < Dashes.Count; i++)
                {
                    MainGrid.Children.Add(Dashes[i]);
                }
        }



        //  int N_el = 5;
        public double W_full = 1000;

        public TimeSpan T_el = TimeSpan.FromSeconds(60);
        public TimeSpan T_full = TimeSpan.FromSeconds(450);


        public double W_el
        {
            get
            {
                double tmp = W_full * T_el.TotalMilliseconds / T_full.TotalMilliseconds;
                if (tmp < 0) tmp = 0;
                return (tmp);
            }
        }


        public double N_el
        {
            get
            {
                double tmp = T_full.TotalMilliseconds / T_el.TotalMilliseconds;
                if (tmp < 0) tmp = 0;
                return (tmp);
            }
        }


        //две извращенческие функции.... 
        public void ChangeDashesWidth(double width)
        {
            foreach (var item in MainGrid.Children)
            {
                if (!(item is Dash)) continue;
                Dash dash = (item as Dash);

                dash.LineWidth = width;
            }
        }

        public void ChangeDashesHeight(double height)
        {
            foreach (var item in MainGrid.Children)
            {
                if (!(item is Dash)) continue;
                Dash dash = (item as Dash);

                dash.LineHeight = height;
            }
        }

        public void HideRepeatedDashes(IEnumerable<Dash> AlreadyCreatedDashes)
        {
            foreach (Dash d in Dashes)
                foreach (Dash oldD in AlreadyCreatedDashes)
                    if (d.Time == oldD.Time)
                        d.Opacity = 0;
        }

        public void addDash(int i)
        {
            Dash d = new Dash();
            d.Time = TimeSpan.FromSeconds(T_el.TotalSeconds * ((double)(i - 1)));
            RefreshBinding(d);
            Dashes.Add(d);
            MainGrid.Children.Add(d);

        }

        public void ClearDashes()
        {
            Dashes.Clear();
            MainGrid.Children.Clear();
        }

        public void FillDashes(int num)
        {
            if (MainGrid.Children.Count < num)
            {
                int N = MainGrid.Children.Count - num;
                for (int i = 1; i < num; i++)
                {
                    addDash(i);
                }
            }
        }



        double scale = 1;
        public double Scale
        {
            get
            {
                return scale;
            }

            set
            {
                if (value <= 0.001) return;
                scale = value;
                foreach (var item in MainGrid.Children)
                {
                    if (!(item is Dash)) continue;
                    Dash dash = (item as Dash);

                    double nextwidth = scale * this.ActualWidth / N_el;
                    ScaleAnimation(dash, dash.ActualWidth, nextwidth, (s, ee) =>
                    {
                        RefreshBinding(dash);

                        dash.BeginAnimation(Dash.WidthProperty, null);
                    });

                }

            }
        }
        void ScaleAnimation(Dash d, double From, double To, EventHandler eventHandler)
        {
            DoubleAnimation a = new DoubleAnimation();
            a.From = From;
            a.To = To;
            a.Duration = TimeSpan.FromSeconds(0.1);
            a.Completed += eventHandler;
            d.BeginAnimation(Dash.WidthProperty, a);
        }



        void RefreshBinding(Dash dash)
        {
            Binding binding = new Binding();
            binding.Source = this;  // элемент-источник
            binding.Converter = new WidthConverteEx();
            binding.ConverterParameter = new WidthConverterParameterEx(this);
            binding.Path = new PropertyPath("ActualWidth"); // свойство элемента-источника
            dash.SetBinding(Dash.WidthProperty, binding); // установка привязки для элемента-приемника

            Binding bindingTimeLabelVis = new Binding();
            bindingTimeLabelVis.Source = this;  // элемент-источник
            bindingTimeLabelVis.Path = new PropertyPath("TimeLabelVisibility"); // свойство элемента-источника
            dash.SetBinding(Dash.TimeLabelVisibilityProperty, bindingTimeLabelVis); // установка привязки для элемента-приемника

        }



        class WidthConverteEx : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (((WidthConverterParameterEx)parameter).Scale * (double)value / (((WidthConverterParameterEx)parameter).ElementsCount));
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return DependencyProperty.UnsetValue;
            }
        }

    }


    public class WidthConverterParameterEx
    {
        TimeDashesArea TimeLine;
        public WidthConverterParameterEx(TimeDashesArea timeLine)
        {
            TimeLine = timeLine;
        }

        public double ElementsCount
        {
            get
            {
                return TimeLine.N_el;
            }
        }
        public double Scale
        {
            get
            {
                return TimeLine.Scale;
            }
        }
    }
}
