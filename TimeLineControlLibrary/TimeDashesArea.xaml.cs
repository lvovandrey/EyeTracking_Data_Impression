using System;
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
            Dashes = new List<Dash>();
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

        public List<Dash> Dashes;


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

        public TimeSpan T_el = TimeSpan.FromSeconds(60);
        public TimeSpan T_full = TimeSpan.FromSeconds(450);

        public void ChangeDashesWidth(double width)
        {
            foreach (var dash in Dashes)
                dash.LineWidth = width;
        }

        public void ChangeDashesHeight(double height)
        {
            foreach (var dash in Dashes)
                dash.LineHeight = height;
        }

        public void HideRepeatedDashes(IEnumerable<Dash> AlreadyCreatedDashes)
        {
            foreach (Dash d in Dashes)
                foreach (Dash oldD in AlreadyCreatedDashes)
                    if (d.Time == oldD.Time)
                        d.Opacity = 0;
        }


        public double DashHeight, DashWidth;

        public void DrawDash(TimeSpan time)
        {
            if (Dashes.Where(dash => dash.Time == time).Count() > 0) return;
            Dash d = new Dash();
            d.Time = time;
            // RefreshBinding(d);
            d.LineHeight = DashHeight;
            d.LineWidth = DashWidth;
            d.Margin = CalculateDashMargin(d);
            AddDash(d);
        }

        private void AddDash(Dash dash)
        {
            Dashes.Add(dash);
            MainGrid.Children.Add(dash);
        }

        private void RemoveDash(Dash dash)
        {
            Dashes.Remove(dash);
            MainGrid.Children.Remove(dash);
        }

        public void ClearAllDashes()
        {
            Dashes.Clear();
            MainGrid.Children.Clear();
        }

        private Thickness CalculateDashMargin(Dash dash)
        {
            double horisontal_offset = this.ActualWidth * dash.Time.TotalSeconds / T_full.TotalSeconds;
            return new Thickness(horisontal_offset, 0, 0, 0);
        }

        public void DrawAllDashesInInterval(TimeSpan timeBegin, TimeSpan timeEnd)
        {
            TimeSpan timeFirst = timeBegin - TimeSpan.FromSeconds(timeBegin.TotalSeconds % T_el.TotalSeconds) + T_el; //время первого видимого dash в интервале
            TimeSpan timeLast = timeEnd - TimeSpan.FromSeconds(timeEnd.TotalSeconds % T_el.TotalSeconds); //время последнего видимого dash в интервале
            TimeSpan timeDuration = timeLast - timeFirst;//Длительность от первого до последнего dash
            int NDashes = (int)Math.Round(timeDuration.TotalSeconds / T_el.TotalSeconds)+1; //это кол-во dash'ей (на всякий случай округлил все)
            for (int i = 0; i < NDashes; i++)
            {
                TimeSpan timeCurDash = TimeSpan.FromSeconds(i * T_el.TotalSeconds + timeFirst.TotalSeconds);
                DrawDash(timeCurDash);
            }
        }

        public void EraseDashesInInterval(TimeSpan timeBegin, TimeSpan timeEnd)
        {
            List<Dash> dashes = FindDashesInInterval(timeBegin, timeEnd);
            foreach (var dash in dashes)
            {
                RemoveDash(dash);
            }
        }



        public List<Dash> FindDashesInInterval(TimeSpan timeBegin, TimeSpan timeEnd)
        {
            List<Dash> FindedDashes = new List<Dash>();
            foreach (var d in Dashes)
               if (d.Time >= timeBegin && d.Time <= timeEnd)
                        FindedDashes.Add(d);

            return FindedDashes;
        }

        void RefreshBinding(Dash dash)
        {

            //Binding bindingTimeLabelVis = new Binding();
            //bindingTimeLabelVis.Source = this;  // элемент-источник
            //bindingTimeLabelVis.Path = new PropertyPath("TimeLabelVisibility"); // свойство элемента-источника
            //dash.SetBinding(Dash.TimeLabelVisibilityProperty, bindingTimeLabelVis); // установка привязки для элемента-приемника

        }
    }

}
