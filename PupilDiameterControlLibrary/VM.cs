using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            OnPropertyChanged("SeriesBase");
        }

        public PupilDiameterUI PupilDiameterUI;


        List<double> PupilDiameterLeft
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

        List<double> PupilDiameterRight
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

        List<TimeSpan> Times
        {
            get
            {
                var list = new List<TimeSpan>();
                foreach (var item in PupilDiameterUI.PupilDiameter)
                {
                    list.Add(item.Time);
                }
                return list;
            }
        }

        public SeriesCollection SeriesBase
        {
            get
            {
                return new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Right Eye",
                    Values = new ChartValues<double> (PupilDiameterRight),
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Left Eye",
                    Values = new ChartValues<double> (PupilDiameterLeft),
                    PointGeometry = null
                }
            };
            }

        }
        public SeriesCollection SeriesEyesDelta { get; set; }

        private double Min = 0;
        private double Max = 0;
        public SeriesCollection SeriesBoxPlot { get; set; }
        public string[] XLable { get; private set; }
        public Func<double, string> YFormatter { get; set; }



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
