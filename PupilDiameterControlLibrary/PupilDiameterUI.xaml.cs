using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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

namespace PupilDiameterControlLibrary
{
    public delegate void PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e);

    /// <summary>
    /// Interaction logic for PupilDiameterUI.xaml
    /// </summary>
    public partial class PupilDiameterUI : UserControl, INotifyPropertyChanged
    {
        public PupilDiameterUI()
        {
            InitializeComponent();
            VM = new VM(this);
        }

        public VM VM;

        private void PupilDiameterUI_OnPupilDiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

     

        #region DependencyProperty PupilDiameter
        //DependencyProperty PupilDiameter - чтобы можно было подписаться на него
        public ObservableCollection<TimeSpan_PupilDiameters_Pair> PupilDiameter
        {
            get { return (ObservableCollection<TimeSpan_PupilDiameters_Pair>)GetValue(PupilDiameterProperty); }
            set
            {
                SetValue(PupilDiameterProperty, value);
            }
        }

        public static readonly DependencyProperty PupilDiameterProperty =
            DependencyProperty.Register("PupilDiameter", typeof(ObservableCollection<TimeSpan_PupilDiameters_Pair>), typeof(PupilDiameterUI), 
                new PropertyMetadata(new ObservableCollection<TimeSpan_PupilDiameters_Pair>(), new PropertyChangedCallback(PupilDiameterPropertyChangedCallback)));

        public event PropertyChanged OnPupilDiameterChanged;

        private static void PupilDiameterPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((PupilDiameterUI)d).OnPupilDiameterChanged != null)
                ((PupilDiameterUI)d).OnPupilDiameterChanged(d, e);
        }
        #endregion





        //public SeriesCollection SeriesBase { get; set; }
        //public SeriesCollection SeriesEyesDelta { get; set; }

        //private double Min = 0;
        //private double Max = 0;
        //public SeriesCollection SeriesBoxPlot { get; set; }
        //public string[] XLable { get; private set; }
        //public Func<double, string> YFormatter { get; set; }







        //void UpdateBoxPlot(double min, double max)
        //{
        //    var minIndex = Math.Max(Times.FindIndex(x => x >= min), 0);
        //    var maxIndex = Times.FindLastIndex(x => x <= max);
        //    maxIndex = maxIndex >= 0 ? Math.Max(maxIndex, minIndex) : Times.Count - 1;

        //    List<double> newLeftEye = LeftEye.GetRange(minIndex, maxIndex - minIndex + 1);
        //    List<double> newRightEye = RightEye.GetRange(minIndex, maxIndex - minIndex + 1);

        //    SeriesBoxPlot.First().Values = new ChartValues<OhlcPoint>
        //    {
        //    GetOhlcPointFrom(newLeftEye),
        //    GetOhlcPointFrom(newRightEye),
        //    };
        //}

        //static OhlcPoint GetOhlcPointFrom(List<double> list)
        //{
        //    return new OhlcPoint(Quartile(list.ToArray(), 1), list.Max(), list.Min(), Quartile(list.ToArray(), 3));
        //}

        //internal static double Quartile(double[] array, int nth_quartile)
        //{
        //    if (array.Length == 0) return 0;
        //    if (array.Length == 1) return 1;
        //    Array.Sort(array);
        //    double dblPercentage = 0;

        //    switch (nth_quartile)
        //    {
        //        case 0:
        //            dblPercentage = 0; //Smallest value in the data set
        //            break;
        //        case 1:
        //            dblPercentage = 25; //First quartile (25th percentile)
        //            break;
        //        case 2:
        //            dblPercentage = 50; //Second quartile (50th percentile)
        //            break;

        //        case 3:
        //            dblPercentage = 75; //Third quartile (75th percentile)
        //            break;

        //        case 4:
        //            dblPercentage = 100; //Largest value in the data set
        //            break;
        //        default:
        //            dblPercentage = 0;
        //            break;
        //    }


        //    if (dblPercentage >= 100.0d) return array[array.Length - 1];

        //    double position = (double)(array.Length + 1) * dblPercentage / 100.0;
        //    double leftNumber = 0.0d, rightNumber = 0.0d;

        //    double n = dblPercentage / 100.0d * (array.Length - 1) + 1.0d;

        //    if (position >= 1)
        //    {
        //        leftNumber = array[(int)System.Math.Floor(n) - 1];
        //        rightNumber = array[(int)System.Math.Floor(n)];
        //    }
        //    else
        //    {
        //        leftNumber = array[0]; // first data
        //        rightNumber = array[1]; // first data
        //    }

        //    if (leftNumber == rightNumber)
        //        return leftNumber;
        //    else
        //    {
        //        double part = n - System.Math.Floor(n);
        //        return leftNumber + part * (rightNumber - leftNumber);
        //    }
        //}

        //private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (DataContext != null) return;
        //}
        //private void Min_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    //TODO улучшить обработчик исключений приведения типа
        //    try
        //    {
        //        Min = double.Parse(Min_TextBox.Text, CultureInfo.InvariantCulture);
        //    }
        //    catch
        //    {
        //        Min_TextBox.Clear();
        //        Min = 0;
        //    }

        //    UpdateBoxPlot(Min, Max);
        //}

        //private void Max_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        Max = Convert.ToDouble(Max_TextBox.Text);
        //    }
        //    catch
        //    {
        //        Max_TextBox.Clear();
        //        Max = Times.Last();
        //    }
        //    UpdateBoxPlot(Min, Max);
        //}


        #region mvvm
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Min_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }

}
