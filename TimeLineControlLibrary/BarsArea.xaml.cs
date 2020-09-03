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
    /// Логика взаимодействия для BarsArea.xaml
    /// </summary>
    public partial class BarsArea : UserControl, INotifyPropertyChanged
    {
        public BarsArea()
        {
            InitializeComponent();
        }

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
            DependencyProperty.Register("FullTime", typeof(TimeSpan), typeof(BarsArea), new PropertyMetadata(TimeSpan.FromSeconds(10)));




        ObservableCollection<Bar> Bars = new ObservableCollection<Bar>();

        public void CalcBarPosition(Bar bar)
        {
            double widthFull = this.ActualWidth;
            bar.Body.Width = (bar.Duration().TotalMinutes / FullTime.TotalMinutes) * widthFull;
            bar.Body.Margin = new Thickness((bar.TimeBegin.TotalMinutes / FullTime.TotalMinutes) * widthFull, 0, 0, 0);
        }

        public void AddBar(Bar bar)
        {
            CalcBarPosition(bar);
            Bars.Add(bar);
            GridMain.Children.Add(bar.Body);
        }

        public void ClearBars()
        {
            Bars.Clear();
            GridMain.Children.Clear();
        }



        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }
}
