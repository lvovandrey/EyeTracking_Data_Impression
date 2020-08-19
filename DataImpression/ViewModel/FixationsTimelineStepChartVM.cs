using DataImpression.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataImpression.ViewModel
{
    public class FixationsTimelineStepChartVM : DocumentBodyVM
    {
        #region ctor
        public FixationsTimelineStepChartVM(Model model) : base(model)
        {
        }
        #endregion

        #region Fields
        #endregion


        #region Properties
       
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



                return new SeriesCollection
                {
                    new StepLineSeries
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
