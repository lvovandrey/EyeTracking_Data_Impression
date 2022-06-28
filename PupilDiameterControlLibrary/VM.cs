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
            OnPropertyChanged("SeriesBoxPlot");
            OnPropertyChanged("IntervalBegin");
            OnPropertyChanged("IntervalEnd");

            OnPropertyChanged("SeriesTestSpectre");
            OnPropertyChanged("SeriesTestSignalSpectre");
            OnPropertyChanged("SeriesLeftEyeSpectre");
            OnPropertyChanged("SeriesRightEyeSpectre");

            OnPropertyChanged("SeriesEyesDelta");
            OnPropertyChanged("SeriesEyesDeltaHistogram");
            OnPropertyChanged("EyesDeltaAvg");
            OnPropertyChanged("EyesDeltaSD");
            OnPropertyChanged("EyesDeltaAvgPlusSD");
            OnPropertyChanged("EyesDeltaAvgMinusSD");
            OnPropertyChanged("SeriesEyesDeltaHistogramLabels");

            OnPropertyChanged("SeriesLeftEye");
            OnPropertyChanged("SeriesLeftEyeHistogram");
            OnPropertyChanged("LeftEyeAvg");
            OnPropertyChanged("LeftEyeSD");
            OnPropertyChanged("LeftEyeAvgPlusSD");
            OnPropertyChanged("LeftEyeAvgMinusSD");
            OnPropertyChanged("SeriesLeftEyeHistogramLabels");

            OnPropertyChanged("SeriesRightEye");
            OnPropertyChanged("SeriesRightEyeHistogram");
            OnPropertyChanged("RightEyeAvg");
            OnPropertyChanged("RightEyeSD");
            OnPropertyChanged("RightEyeAvgPlusSD");
            OnPropertyChanged("RightEyeAvgMinusSD");
            OnPropertyChanged("SeriesRightEyeHistogramLabels");
        }

        public PupilDiameterUI PupilDiameterUI;


        public Func<double, string> FormatterTimeMMSS { get { return val => TimeSpan.FromSeconds(val).ToString(@"mm\:ss"); } }
        public Func<double, string> FormatterDoubleC2 { get { return val => val.ToString("0.00"); } }
        public Func<double, string> FormatterInt { get { return val => val.ToString("#"); } }
        public Func<double, string> FormatterFromString { get { return value => value.ToString("N"); } }



        public SeriesCollection SeriesBase
        {
            get
            {
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

        #region EyesDelta
        public double EyesDeltaAvg => PupilDiameterUI.PupilDiameter.Average(item => item.DeltaLeftRight);
        public double EyesDeltaSD => getStandardDeviation(PupilDiameterUI.PupilDiameter.Select(item => item.DeltaLeftRight).ToList());
        public double EyesDeltaSDx2 => EyesDeltaSD * 2;
        public double EyesDeltaSDx4 => EyesDeltaSD * 4;
        public double EyesDeltaSDx6 => EyesDeltaSD * 6;
        public double EyesDeltaAvgPlusSD => EyesDeltaAvg + EyesDeltaSD;
        public double EyesDeltaAvgPlus2SD => EyesDeltaAvg + 2 * EyesDeltaSD;
        public double EyesDeltaAvgPlus3SD => EyesDeltaAvg + 3 * EyesDeltaSD;

        public double EyesDeltaAvgMinusSD => EyesDeltaAvg - EyesDeltaSD;
        public double EyesDeltaAvgMinus2SD => EyesDeltaAvg - 2 * EyesDeltaSD;
        public double EyesDeltaAvgMinus3SD => EyesDeltaAvg - 3 * EyesDeltaSD;
        public SeriesCollection SeriesEyesDelta
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                List<ObservablePoint> xy_DeltaPupil = new List<ObservablePoint>();
                List<ObservablePoint> xy_DeltaPupilAvg = new List<ObservablePoint>();
                List<ObservablePoint> xy_DeltaPupilSD = new List<ObservablePoint>();
                List<ObservablePoint> xy_DeltaPupilSD_ = new List<ObservablePoint>();
                List<ObservablePoint> xy_DeltaPupil2SD = new List<ObservablePoint>();
                List<ObservablePoint> xy_DeltaPupil2SD_ = new List<ObservablePoint>();
                List<ObservablePoint> xy_DeltaPupil3SD = new List<ObservablePoint>();
                List<ObservablePoint> xy_DeltaPupil3SD_ = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    xy_DeltaPupil.Add(new ObservablePoint(item.Time.TotalSeconds, (item.PupilDiameterLeft - item.PupilDiameterRight)));
                }


                if (PupilDiameterUI.PupilDiameter.Count > 0)
                {
                    xy_DeltaPupilAvg.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, EyesDeltaAvg));
                    xy_DeltaPupilAvg.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, EyesDeltaAvg));

                    xy_DeltaPupilSD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, EyesDeltaAvgPlusSD));
                    xy_DeltaPupilSD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, EyesDeltaAvgPlusSD));
                    xy_DeltaPupilSD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, EyesDeltaAvgMinusSD));
                    xy_DeltaPupilSD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, EyesDeltaAvgMinusSD));

                    xy_DeltaPupil2SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, EyesDeltaAvgPlus2SD));
                    xy_DeltaPupil2SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, EyesDeltaAvgPlus2SD));
                    xy_DeltaPupil2SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, EyesDeltaAvgMinus2SD));
                    xy_DeltaPupil2SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, EyesDeltaAvgMinus2SD));

                    xy_DeltaPupil3SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, EyesDeltaAvgPlus3SD));
                    xy_DeltaPupil3SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, EyesDeltaAvgPlus3SD));
                    xy_DeltaPupil3SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, EyesDeltaAvgMinus3SD));
                    xy_DeltaPupil3SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, EyesDeltaAvgMinus3SD));
                }




                var result = new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Delta Diameters",
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupil),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Gold), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "MTO = " + EyesDeltaAvg.ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupilAvg),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Red), StrokeThickness=3, Fill=  new SolidColorBrush(Colors.Transparent)
                    }
                    ,
                    new LineSeries
                    {
                    Title= "\u03C3 = " + EyesDeltaSD.ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupilSD),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.DarkGreen), Fill= new SolidColorBrush(Colors.Transparent)
                    }
                    ,
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupilSD_),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.DarkGreen), Fill=  new SolidColorBrush(Colors.Transparent)
                    }
                    ,                    new LineSeries
                    {
                    Title= "2*\u03C3 = " + (2*EyesDeltaSD).ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupil2SD),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Green), Fill= new SolidColorBrush(Colors.Transparent)
                    }
                    ,
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupil2SD_),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Green), Fill=  new SolidColorBrush(Colors.Transparent)
                    }
                    ,                    new LineSeries
                    {
                    Title= "3*\u03C3 = " + (3*EyesDeltaSD).ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupil3SD),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.LightGreen), Fill= new SolidColorBrush(Colors.Transparent)
                    }
                    ,
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_DeltaPupil3SD_),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.LightGreen), Fill=  new SolidColorBrush(Colors.Transparent)
                    }


                };

                List<ObservablePoint> BiggestThan3SD = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    if (item.DeltaLeftRight > EyesDeltaAvgPlus3SD)
                        BiggestThan3SD.Add(new ObservablePoint(item.Time.TotalSeconds, item.DeltaLeftRight));
                }


                if (BiggestThan3SD.Count > 0)
                {
                    result.Add(new LineSeries
                    {
                        Title = "> 3*\u03C3 ",
                        Values = new ChartValues<ObservablePoint>(BiggestThan3SD),
                        LineSmoothness = 0,
                        PointGeometry = DefaultGeometries.Circle,
                        StrokeThickness = 0,
                        Stroke = new SolidColorBrush(Colors.Blue),
                        Fill = new SolidColorBrush(Colors.Transparent)
                    });
                }

                return result;
            }
        }

        public string[] SeriesEyesDeltaHistogramLabels { get; private set; }

        public SeriesCollection SeriesEyesDeltaHistogram
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                var colsCount = 10;
                var max = PupilDiameterUI.PupilDiameter.Select(item => item.DeltaLeftRight).Max();
                var min = PupilDiameterUI.PupilDiameter.Select(item => item.DeltaLeftRight).Min();
                var stage = (max - min) / colsCount;
                double[] histogram = new double[colsCount + 1];
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {

                    for (int i = 0; i < colsCount; i++)
                    {
                        if (item.DeltaLeftRight >= (min + stage * i) && item.DeltaLeftRight < (min + stage * (i + 1)))
                            histogram[i]++;
                        if (i == colsCount && item.DeltaLeftRight == (min + stage * (i + 1)))
                            histogram[i]++;
                    }
                }
                SeriesEyesDeltaHistogramLabels = new string[colsCount];
                for (int i = 0; i < colsCount; i++)
                {
                    SeriesEyesDeltaHistogramLabels[i] = "[" + (min + stage * i).ToString("0.000") + " | " + (min + stage * (i + 1)).ToString("0.000") + "]";
                }


                return new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "2015",
                    Values = new ChartValues<double> (histogram)
                }
                };

            }
        }
        #endregion



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

        private double getStandardDeviation(List<double> doubleList)
        {
            double average = doubleList.Average();
            double sumOfDerivation = 0;
            foreach (double value in doubleList)
            {
                sumOfDerivation += (value) * (value);
            }
            double sumOfDerivationAverage = sumOfDerivation / (doubleList.Count - 1);
            return Math.Sqrt(sumOfDerivationAverage - (average * average));
        }

        #region TestSpectre
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
                    + 3 * Math.Sin((double)i / 4000)));
            }
            if (x.Count == 0) x.Add(0);
            return x;
        }

        public SeriesCollection SeriesTestSignalSpectre
        {
            get
            {
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

        public SeriesCollection SeriesTestSpectre
        {
            get
            {
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
        #endregion

        #region Spectre Left
        public SeriesCollection SeriesLeftEyeSpectre
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                List<double> x = PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterLeft).ToList();
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
                    ii++; if (ii > 100) break;
                    xgraph.Add(new ObservablePoint(ii, _f.x / max));
                }
                ii = 0;
                max = f.Max(item => item.y);
                foreach (var _f in f)
                {
                    ii++; if (ii > 100) break;
                    ygraph.Add(new ObservablePoint(ii, _f.y / max));
                }


                return new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Величина сигнала",
                    Values = new ChartValues<ObservablePoint>(xgraph),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Gold)
                    }
                    ,
                    new LineSeries
                    {
                    Title= "Величина шумов",
                    Values = new ChartValues<ObservablePoint>(ygraph),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Red)
                    }
                };
            }
        }
        #endregion

        #region Spectre Right
        public SeriesCollection SeriesRightEyeSpectre
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                List<double> x = PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterRight).ToList();
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
                    ii++; if (ii > 100) break;
                    xgraph.Add(new ObservablePoint(ii, _f.x / max));
                }
                ii = 0;
                max = f.Max(item => item.y);
                foreach (var _f in f)
                {
                    ii++; if (ii > 100) break;
                    ygraph.Add(new ObservablePoint(ii, _f.y / max));
                }


                return new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Величина сигнала",
                    Values = new ChartValues<ObservablePoint>(xgraph),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Gold)
                    }
                    ,
                    new LineSeries
                    {
                    Title= "Величина шумов",
                    Values = new ChartValues<ObservablePoint>(ygraph),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Red)
                    }
                };
            }
        }
        #endregion

        #region LeftEye
        public double LeftEyeAvg => PupilDiameterUI.PupilDiameter.Average(item => item.PupilDiameterLeft);
        public double LeftEyeSD => getStandardDeviation(PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterLeft).ToList());
        public double LeftEyeSDx2 => LeftEyeSD * 2;
        public double LeftEyeSDx4 => LeftEyeSD * 4;
        public double LeftEyeSDx6 => LeftEyeSD * 6;
        public double LeftEyeAvgPlusSD => LeftEyeAvg + LeftEyeSD;
        public double LeftEyeAvgPlus2SD => LeftEyeAvg + 2 * LeftEyeSD;
        public double LeftEyeAvgPlus3SD => LeftEyeAvg + 3 * LeftEyeSD;

        public double LeftEyeAvgMinusSD => LeftEyeAvg - LeftEyeSD;
        public double LeftEyeAvgMinus2SD => LeftEyeAvg - 2 * LeftEyeSD;
        public double LeftEyeAvgMinus3SD => LeftEyeAvg - 3 * LeftEyeSD;
        public SeriesCollection SeriesLeftEye
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                List<ObservablePoint> xy_LeftPupil = new List<ObservablePoint>();
                List<ObservablePoint> xy_LeftPupilAvg = new List<ObservablePoint>();
                List<ObservablePoint> xy_LeftPupilSD = new List<ObservablePoint>();
                List<ObservablePoint> xy_LeftPupilSD_ = new List<ObservablePoint>();
                List<ObservablePoint> xy_LeftPupil2SD = new List<ObservablePoint>();
                List<ObservablePoint> xy_LeftPupil2SD_ = new List<ObservablePoint>();
                List<ObservablePoint> xy_LeftPupil3SD = new List<ObservablePoint>();
                List<ObservablePoint> xy_LeftPupil3SD_ = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    xy_LeftPupil.Add(new ObservablePoint(item.Time.TotalSeconds, item.PupilDiameterLeft));
                }


                if (PupilDiameterUI.PupilDiameter.Count > 0)
                {
                    xy_LeftPupilAvg.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, LeftEyeAvg));
                    xy_LeftPupilAvg.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, LeftEyeAvg));

                    xy_LeftPupilSD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, LeftEyeAvgPlusSD));
                    xy_LeftPupilSD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, LeftEyeAvgPlusSD));
                    xy_LeftPupilSD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, LeftEyeAvgMinusSD));
                    xy_LeftPupilSD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, LeftEyeAvgMinusSD));

                    xy_LeftPupil2SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, LeftEyeAvgPlus2SD));
                    xy_LeftPupil2SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, LeftEyeAvgPlus2SD));
                    xy_LeftPupil2SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, LeftEyeAvgMinus2SD));
                    xy_LeftPupil2SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, LeftEyeAvgMinus2SD));

                    xy_LeftPupil3SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, LeftEyeAvgPlus3SD));
                    xy_LeftPupil3SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, LeftEyeAvgPlus3SD));
                    xy_LeftPupil3SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, LeftEyeAvgMinus3SD));
                    xy_LeftPupil3SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, LeftEyeAvgMinus3SD));
                }

                List<ObservablePoint> LeftPupil_BiggestThan3SD = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    if (item.PupilDiameterLeft > LeftEyeAvgPlus3SD)
                        LeftPupil_BiggestThan3SD.Add(new ObservablePoint(item.Time.TotalSeconds, item.PupilDiameterLeft));
                }


                var result =  new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Left Diameters",
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupil),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Gold), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "MTO = " + LeftEyeAvg.ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupilAvg),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Red), StrokeThickness=3, Fill=  new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "\u03C3 = " + LeftEyeSD.ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupilSD),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.DarkGreen), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupilSD_),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.DarkGreen), Fill=  new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "2*\u03C3 = " + (2*LeftEyeSD).ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupil2SD),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Green), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupil2SD_),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Green), Fill=  new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "3*\u03C3 = " + (3*LeftEyeSD).ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupil3SD),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.LightGreen), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_LeftPupil3SD_),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.LightGreen), Fill=  new SolidColorBrush(Colors.Transparent)
                    }

                };

                if (LeftPupil_BiggestThan3SD.Count > 0)
                {
                    result.Add(new LineSeries
                    {
                        Title = "> 3*\u03C3 ",
                        Values = new ChartValues<ObservablePoint>(LeftPupil_BiggestThan3SD),
                        LineSmoothness = 0,
                        PointGeometry = DefaultGeometries.Circle,
                        StrokeThickness = 0,
                        Stroke = new SolidColorBrush(Colors.Blue),
                        Fill = new SolidColorBrush(Colors.Transparent)
                    });
                }

                return result;
            }
        }

        public string[] SeriesLeftEyeHistogramLabels { get; private set; }

        public SeriesCollection SeriesLeftEyeHistogram
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                var colsCount = 10;
                var max = PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterLeft).Max();
                var min = PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterLeft).Min();
                var stage = (max - min) / colsCount;
                double[] histogram = new double[colsCount + 1];
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {

                    for (int i = 0; i < colsCount; i++)
                    {
                        if (item.PupilDiameterLeft >= (min + stage * i) && item.PupilDiameterLeft < (min + stage * (i + 1)))
                            histogram[i]++;
                        if (i == colsCount && item.PupilDiameterLeft == (min + stage * (i + 1)))
                            histogram[i]++;
                    }
                }
                SeriesLeftEyeHistogramLabels = new string[colsCount];
                for (int i = 0; i < colsCount; i++)
                {
                    SeriesLeftEyeHistogramLabels[i] = "[" + (min + stage * i).ToString("0.000") + " | " + (min + stage * (i + 1)).ToString("0.000") + "]";
                }


                return new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "",
                    Values = new ChartValues<double> (histogram)
                }
                };

            }
        }

        #endregion





        #region RightEye
        public double RightEyeAvg => PupilDiameterUI.PupilDiameter.Average(item => item.PupilDiameterRight);
        public double RightEyeSD => getStandardDeviation(PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterRight).ToList());
        public double RightEyeSDx2 => RightEyeSD * 2;
        public double RightEyeSDx4 => RightEyeSD * 4;
        public double RightEyeSDx6 => RightEyeSD * 6;
        public double RightEyeAvgPlusSD => RightEyeAvg + RightEyeSD;
        public double RightEyeAvgPlus2SD => RightEyeAvg + 2 * RightEyeSD;
        public double RightEyeAvgPlus3SD => RightEyeAvg + 3 * RightEyeSD;

        public double RightEyeAvgMinusSD => RightEyeAvg - RightEyeSD;
        public double RightEyeAvgMinus2SD => RightEyeAvg - 2 * RightEyeSD;
        public double RightEyeAvgMinus3SD => RightEyeAvg - 3 * RightEyeSD;
        public SeriesCollection SeriesRightEye
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                List<ObservablePoint> xy_RightPupil = new List<ObservablePoint>();
                List<ObservablePoint> xy_RightPupilAvg = new List<ObservablePoint>();
                List<ObservablePoint> xy_RightPupilSD = new List<ObservablePoint>();
                List<ObservablePoint> xy_RightPupilSD_ = new List<ObservablePoint>();
                List<ObservablePoint> xy_RightPupil2SD = new List<ObservablePoint>();
                List<ObservablePoint> xy_RightPupil2SD_ = new List<ObservablePoint>();
                List<ObservablePoint> xy_RightPupil3SD = new List<ObservablePoint>();
                List<ObservablePoint> xy_RightPupil3SD_ = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    xy_RightPupil.Add(new ObservablePoint(item.Time.TotalSeconds, item.PupilDiameterRight));
                }


                if (PupilDiameterUI.PupilDiameter.Count > 0)
                {
                    xy_RightPupilAvg.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, RightEyeAvg));
                    xy_RightPupilAvg.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, RightEyeAvg));

                    xy_RightPupilSD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, RightEyeAvgPlusSD));
                    xy_RightPupilSD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, RightEyeAvgPlusSD));
                    xy_RightPupilSD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, RightEyeAvgMinusSD));
                    xy_RightPupilSD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, RightEyeAvgMinusSD));

                    xy_RightPupil2SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, RightEyeAvgPlus2SD));
                    xy_RightPupil2SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, RightEyeAvgPlus2SD));
                    xy_RightPupil2SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, RightEyeAvgMinus2SD));
                    xy_RightPupil2SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, RightEyeAvgMinus2SD));

                    xy_RightPupil3SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, RightEyeAvgPlus3SD));
                    xy_RightPupil3SD.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, RightEyeAvgPlus3SD));
                    xy_RightPupil3SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.First().Time.TotalSeconds, RightEyeAvgMinus3SD));
                    xy_RightPupil3SD_.Add(new ObservablePoint(PupilDiameterUI.PupilDiameter.Last().Time.TotalSeconds, RightEyeAvgMinus3SD));
                }

                List<ObservablePoint> RightPupil_BiggestThan3SD = new List<ObservablePoint>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    if (item.PupilDiameterRight > RightEyeAvgPlus3SD)
                        RightPupil_BiggestThan3SD.Add(new ObservablePoint(item.Time.TotalSeconds, item.PupilDiameterRight));
                }



                var result =  new SeriesCollection
                {
                    new LineSeries
                    {
                    Title= "Right Diameters",
                    Values = new ChartValues<ObservablePoint>(xy_RightPupil),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Gold), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "MTO = " + RightEyeAvg.ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_RightPupilAvg),
                    LineSmoothness = 0 ,  PointGeometry = null, Stroke = new SolidColorBrush(Colors.Red), StrokeThickness=3, Fill=  new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "\u03C3 = " + RightEyeSD.ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_RightPupilSD),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.DarkGreen), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_RightPupilSD_),
                    LineSmoothness = 0 ,  PointGeometry = null, StrokeThickness=1, Stroke = new SolidColorBrush(Colors.DarkGreen), Fill=  new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "2*\u03C3 = " + (2*RightEyeSD).ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_RightPupil2SD),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Green), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_RightPupil2SD_),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.Green), Fill=  new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "3*\u03C3 = " + (3*RightEyeSD).ToString("0.000") ,
                    Values = new ChartValues<ObservablePoint>(xy_RightPupil3SD),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.LightGreen), Fill= new SolidColorBrush(Colors.Transparent)
                    },
                    new LineSeries
                    {
                    Title= "" ,
                    Values = new ChartValues<ObservablePoint>(xy_RightPupil3SD_),
                    LineSmoothness = 0 ,  PointGeometry = null,StrokeThickness=1, Stroke = new SolidColorBrush(Colors.LightGreen), Fill=  new SolidColorBrush(Colors.Transparent)
                    }
                    ,


                };

                if (RightPupil_BiggestThan3SD.Count > 0)
                {
                    result.Add(new LineSeries
                    {
                        Title = "> 3*\u03C3 ",
                        Values = new ChartValues<ObservablePoint>(RightPupil_BiggestThan3SD),
                        LineSmoothness = 0,
                        PointGeometry = DefaultGeometries.Circle,
                        StrokeThickness = 0,
                        Stroke = new SolidColorBrush(Colors.Blue),
                        Fill = new SolidColorBrush(Colors.Transparent)
                    });
                }

                return result;
            }
        }

        public string[] SeriesRightEyeHistogramLabels { get; private set; }

        public SeriesCollection SeriesRightEyeHistogram
        {
            get
            {
                if (PupilDiameterUI.PupilDiameter.Count < 1) return null;
                var colsCount = 10;
                var max = PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterRight).Max();
                var min = PupilDiameterUI.PupilDiameter.Select(item => item.PupilDiameterRight).Min();
                var stage = (max - min) / colsCount;
                double[] histogram = new double[colsCount + 1];
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {

                    for (int i = 0; i < colsCount; i++)
                    {
                        if (item.PupilDiameterRight >= (min + stage * i) && item.PupilDiameterRight < (min + stage * (i + 1)))
                            histogram[i]++;
                        if (i == colsCount && item.PupilDiameterRight == (min + stage * (i + 1)))
                            histogram[i]++;
                    }
                }
                SeriesRightEyeHistogramLabels = new string[colsCount];
                for (int i = 0; i < colsCount; i++)
                {
                    SeriesRightEyeHistogramLabels[i] = "[" + (min + stage * i).ToString("0.000") + " | " + (min + stage * (i + 1)).ToString("0.000") + "]";
                }


                return new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "",
                    Values = new ChartValues<double> (histogram)
                }
                };

            }
        }

        #endregion


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
