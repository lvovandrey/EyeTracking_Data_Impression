using DataImpression.AbstractMVVM;
using DataImpression.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DataImpression.ViewModel
{
    public class FAOIDiagramVM : INPCBase
    {
        #region ctor
        public FAOIDiagramVM(Model model)
        {
            _model = model;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;

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
                var values = _model.Results.TimePercentDistribution.Results.Select(r => r.Value);

                return new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = _model.Results.TimePercentDistribution.ParameterName,
                        Values = new ChartValues<double> (values),
                        Fill = Brushes.Blue
                    }
                };
            }
        }
        public string[] Labels
        {
            get
            {
                var faoi_titles = _model.Results.TimePercentDistribution.Results.Select(r => r.FAOI.Name).ToArray();
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
