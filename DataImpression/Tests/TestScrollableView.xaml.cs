using DataImpression.AbstractMVVM;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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

namespace DataImpression.Tests
{
    public delegate void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);


    /// <summary>
    /// Логика взаимодействия для TestScrollableView.xaml
    /// </summary>
    public partial class TestScrollableView : UserControl
    {
        public TestScrollableView()
        {
            InitializeComponent();

            this.OnWidthContainerChanged += TestScrollableView_OnWidthContainerChanged;
        }

        private void TestScrollableView_OnWidthContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TestScrollableViewModel VM = this.DataContext as TestScrollableViewModel;
            if (VM == null) return;
            VM.WidthContainerInVM = this.WidthContainer;
        }

        private void CartesianChart_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            TestScrollableViewModel VM = this.DataContext as TestScrollableViewModel;
            if (VM == null) return;

            if (e.Delta > 0)
            {
                VM.WidthZoom *= 1.1;
            }
            else
            {
                VM.WidthZoom /= 1.1;
            }

        }


        public static readonly DependencyProperty WidthContainerProperty = DependencyProperty.Register("WidthContainer",
           typeof(double), typeof(TestScrollableView),
           new FrameworkPropertyMetadata(new PropertyChangedCallback(WidthContainerPropertyChangedCallback)));
        public double WidthContainer
        {
            get
            {
                double t = Application.Current.Dispatcher.Invoke(new Func<double>(() =>
                {
                    return (double)GetValue(WidthContainerProperty);
                }));
                return t;
            }
            set
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    SetValue(WidthContainerProperty, value);
                }));
            }
        }
        public event PropertyChanged OnWidthContainerChanged;
        static void WidthContainerPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((TestScrollableView)d).OnWidthContainerChanged != null)
                ((TestScrollableView)d).OnWidthContainerChanged(d, e);
        }

       
    }

    public class TestScrollableViewModel : INPCBase

    {
        public TestScrollableViewModel()
        {
            var temporalCv = new ObservablePoint[1000];

            for (var i = 0; i < 1000; i++)
            {
                temporalCv[i] = new ObservablePoint(i * 2, i);
            }

            var cv = new ChartValues<ObservablePoint>();
            cv.AddRange(temporalCv);


            
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = cv,
                    LineSmoothness = 0,
                    Fill = Brushes.Transparent

                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };


            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0 //0: straight lines, 1: really smooth lines

            });



        }

        private double widthZoom = 1;
        public double WidthZoom
        {
            get { return widthZoom; }
            set { widthZoom = value; OnPropertyChanged("CurWidthChart"); }
        }

        public double CurWidthChart { get { return WidthZoom * WidthContainerInVM; } }

        private double widthContainerInVM = 100;
        public double WidthContainerInVM
        {
            get { return widthContainerInVM; }
            set { widthContainerInVM = value; OnPropertyChanged("CurWidthChart"); }
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }

    }

}
