using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PupilDiameterControlLibrary
{
    public class VM : INotifyPropertyChanged
    {
        public VM(PupilDiameterUI pupilDiameterUI)
        {
            PupilDiameterUI = pupilDiameterUI;
            PupilDiameterUI.MainGrid.DataContext = this;
            PupilDiameterUI.OnPupilDiameterChanged += PupilDiameterUI_OnPupilDiameterChanged;
        }

        private void PupilDiameterUI_OnPupilDiameterChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            Max = Times.Last();
            Min = Times.First();

            OnPropertyChanged("SeriesBase");
            OnPropertyChanged("SeriesEyesDelta");
            OnPropertyChanged("SeriesBoxPlot");
            OnPropertyChanged("IntervalBegin");
            OnPropertyChanged("IntervalEnd");
            OnPropertyChanged("SeriesSpectre");
            OnPropertyChanged("SeriesSignalSpectre");
        }

        public PupilDiameterUI PupilDiameterUI;



        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public Func<double, string> XFormatter2 { get; set; }
        public Func<double, string> YFormatter2 { get; set; }
        public Func<double, string> XFormatter3 { get; set; }
        public Func<double, string> YFormatter3 { get; set; }


        public SeriesCollection SeriesBase
        {
            get
            {
                YFormatter = val => val.ToString("0.00");
                XFormatter = val => TimeSpan.FromSeconds(val).ToString(@"mm\:ss");

                List<ObservablePoint> xy_LeftPupil = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    xy_LeftPupil.Add(new ObservablePoint(item.Time.TotalSeconds, item.PupilDiameterLeft));
                }
                List<ObservablePoint> xy_RightPupil = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    xy_RightPupil.Add(new ObservablePoint(item.Time.TotalSeconds, item.PupilDiameterRight));
                }

                return new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Левый",
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupil),
                    LineSmoothness = 0 ,  PointGeometry = null
                    },
                    new LineSeries
                    {
                    Title= "Правый",
                    Values = new ChartValues<ObservablePoint>(xy_RightPupil),
                    LineSmoothness = 0 ,  PointGeometry = null
                    }
                };
            }
        }


        public SeriesCollection SeriesEyesDelta
        {
            get
            {
                YFormatter2 = val => val.ToString("0.00");
                XFormatter2 = val => TimeSpan.FromSeconds(val).ToString(@"mm\:ss");

                List<ObservablePoint> xy_DeltaPupil = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    xy_DeltaPupil.Add(new ObservablePoint(item.Time.TotalSeconds, (item.PupilDiameterLeft - item.PupilDiameterRight)));
                }


                return new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Delta Diameters",
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupil),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Gold)
                    }
                };
            }
        }



        public string[] XLable { get; private set; }

        List<double> Times
        {
            get
            {
                var times = new List<double>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    times.Add(item.Time.TotalMilliseconds);
                }
                return times;
            }
        }

        List<double> LeftEye
        {
            get
            {
                var list = new List<double>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    list.Add(item.PupilDiameterLeft);
                }
                return list;
            }
        }

        List<double> RightEye
        {
            get
            {
                var list = new List<double>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    list.Add(item.PupilDiameterRight);
                }
                return list;
            }
        }


        private double Min { get; set; }
        public string IntervalBegin
        {
            get
            {
                return Min.ToString();
            }
            set
            {
                double val;
                if (!double.TryParse(value, out val)) val = 0;
                if (val < 0) val = 0;
                if (val > Times.Last()) val = Times.Last();
                Min = val;
                OnPropertyChanged("BoxChartIntervalBegin");
                OnPropertyChanged("SeriesBoxPlot");
            }
        }

        private double Max { get; set; }
        public string IntervalEnd
        {
            get
            {
                return Max.ToString();
            }
            set
            {
                double val;
                if (!double.TryParse(value, out val)) val = 0;
                if (val < 0) val = 0;
                if (val > Times.Last()) val = Times.Last();
                Max = val;
                OnPropertyChanged("BoxChartIntervalEnd");
                OnPropertyChanged("SeriesBoxPlot");
            }
        }



        public SeriesCollection SeriesBoxPlot
        {
            get
            {
                if (Times.Count == 0) return null;
                var minIndex = Math.Max(Times.FindIndex(x => x >= Min), 0);
                var maxIndex = Times.FindLastIndex(x => x <= Max);
                maxIndex = maxIndex >= 0 ? Math.Max(maxIndex, minIndex) : Times.Count - 1;

                List<double> newLeftEye = LeftEye.GetRange(minIndex, maxIndex - minIndex + 1);
                List<double> newRightEye = RightEye.GetRange(minIndex, maxIndex - minIndex + 1);

                XLable = new string[] { "Левый", "Правый" };
                return new SeriesCollection
                {
                    new CandleSeries()
                    {
                        Title= "Box plot",
                        Values = new ChartValues<OhlcPoint>
                        {
                            GetOhlcPointFrom(newLeftEye),
                            GetOhlcPointFrom(newRightEye)
                        },
                        StrokeThickness = 5
                    }
                };
            }
        }


        static OhlcPoint GetOhlcPointFrom(List<double> list)
        {
            return new OhlcPoint(Quartile(list.ToArray(), 1), list.Max(), list.Min(), Quartile(list.ToArray(), 3));
        }

        internal static double Quartile(double[] array, int nth_quartile)
        {
            if (array.Length == 0) return 0;
            if (array.Length == 1) return 1;
            Array.Sort(array);
            double dblPercentage = 0;

            switch (nth_quartile)
            {
                case 0:
                    dblPercentage = 0; //Smallest value in the data set
                    break;
                case 1:
                    dblPercentage = 25; //First quartile (25th percentile)
                    break;
                case 2:
                    dblPercentage = 50; //Second quartile (50th percentile)
                    break;

                case 3:
                    dblPercentage = 75; //Third quartile (75th percentile)
                    break;

                case 4:
                    dblPercentage = 100; //Largest value in the data set
                    break;
                default:
                    dblPercentage = 0;
                    break;
            }


            if (dblPercentage >= 100.0d) return array[array.Length - 1];

            double position = (double)(array.Length + 1) * dblPercentage / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;

            double n = dblPercentage / 100.0d * (array.Length - 1) + 1.0d;

            if (position >= 1)
            {
                leftNumber = array[(int)System.Math.Floor(n) - 1];
                rightNumber = array[(int)System.Math.Floor(n)];
            }
            else
            {
                leftNumber = array[0]; // first data
                rightNumber = array[1]; // first data
            }

            if (leftNumber == rightNumber)
                return leftNumber;
            else
            {
                double part = n - System.Math.Floor(n);
                return leftNumber + part * (rightNumber - leftNumber);
            }
        }


        private List<double> GenerateSignal()
        {
            List<double> x = new List<double>();

            //foreach (var item in PupilDiameterUI.PupilDiameter)
            //{
            //    x.Add(item.PupilDiameterLeft);
            //}
            Random r = new Random();
            for (int i = 0; i < 1256600; i++)
            {
                x.Add(r.NextDouble() + 0.33 * (2 * Math.Sin((double)i / 10000)
                    + Math.Sin((double)i / 5000)
                    + Math.Sin((double)i / 2500)
                    + Math.Sin((double)i / 1250)
                    + 3*Math.Sin((double)i / 4000)));
            }
            if (x.Count==0) x.Add(0);
            return x;
        }

        public SeriesCollection SeriesSignalSpectre
        {
            get
            {
                YFormatter2 = val => val.ToString("0.00");
                XFormatter2 = val => TimeSpan.FromSeconds(val).ToString(@"mm\:ss");

                List<double> x = GenerateSignal();
                List<ObservablePoint> signal_graph = new List<ObservablePoint>();

                int ii = 0;
                foreach (var _x in x)
                {
                    ii++; if (ii > 200000) break;
                    if (ii % 100 == 0)
                        signal_graph.Add(new ObservablePoint(ii, _x));
                }


                return new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Signal",
                    Values = new ChartValues<ObservablePoint>(signal_graph),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.White)
                    }
                };
            }
        }

        public SeriesCollection SeriesSpectre
        {
            get
            {
                YFormatter3 = val => val.ToString("0.00");
                XFormatter3 = val => val.ToString("0.00");


                List<double> x = GenerateSignal();
                var xx = x.ToArray();
                alglib.complex[] f;
                double[] x2;
                alglib.fftr1d(xx, out f);
                alglib.fftr1dinv(f, out x2);

                List<ObservablePoint> xgraph = new List<ObservablePoint>();
                List<ObservablePoint> ygraph = new List<ObservablePoint>();

                int ii = 0;
                double max = f.Max(item => item.x);
                foreach (var _f in f)
                {
                    ii++; if (ii > 1000) break;
                    xgraph.Add(new ObservablePoint(ii, _f.x / max));
                }
                ii = 0;
                max = f.Max(item => item.y);
                foreach (var _f in f)
                {
                    ii++; if (ii > 1000) break;
                    ygraph.Add(new ObservablePoint(ii, _f.y / max));
                }


                return new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "x",
                    Values = new ChartValues<ObservablePoint>(xgraph),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Gold)
                    }
                    ,
                    new LineSeries
                    {
                    Title= "y",
                    Values = new ChartValues<ObservablePoint>(ygraph),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Red)
                    }
                };
            }
        }

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
