using DataImpression.Models;
using DataImpression.Models.ResultTypes;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DataImpression.ViewModel
{
    public class SettingsFAOIDistributedColumnChart
    {
        public Brush Fill;
    }

    public class FAOIDistributedColumnChartVM<T>: DocumentBodyVM
    {
        #region ctor
        public FAOIDistributedColumnChartVM(Model model, FAOIDistributed_Parameter<T> FAOIDistributed_Parameter, SettingsFAOIDistributedColumnChart settings) : base(model)
        {
            fAOIDistributed_Parameter = FAOIDistributed_Parameter;
            this.settings = settings;
        }
        #endregion

        #region Fields
        private FAOIDistributed_Parameter<T> fAOIDistributed_Parameter;
        private SettingsFAOIDistributedColumnChart settings;
        #endregion


        #region Properties
        public string Title { get { return fAOIDistributed_Parameter.ParameterName; } }

        


        Visibility visibility;
        public Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                OnPropertyChanged("Visibility");
            }
        }
        #endregion


        public SeriesCollection SeriesCollection
        {
            get
            {
                var values = new List<double>();
                if (((object)fAOIDistributed_Parameter.Results.First().Value is double))
                    values = fAOIDistributed_Parameter.Results.Select(r => (double)(object)r.Value).ToList();

                if (((object)fAOIDistributed_Parameter.Results.First().Value is TimeSpan))
                    values = fAOIDistributed_Parameter.Results.Select(r => ((TimeSpan)(object)r.Value).TotalSeconds).ToList();


                return new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = fAOIDistributed_Parameter.ParameterName,
                        Values = new ChartValues<double> (values),
                        Fill = settings.Fill
                    },
                };
            }
        }
        public string[] Labels
        {
            get
            {
                var faoi_titles = fAOIDistributed_Parameter.Results.Select(r => r.FAOI.Name).ToArray();
                return faoi_titles;
            }
        }
        public Func<double, string> Formatter
        {
            get { return value => value.ToString("N"); }
        }



        #region Methods
        public bool CanExecuteNextInputStage()
        {
            return true; //   if (FAOIsVM?.Count > 0) return true; else return false;
        }
        #endregion

        #region Commands

        #endregion
    }
}
