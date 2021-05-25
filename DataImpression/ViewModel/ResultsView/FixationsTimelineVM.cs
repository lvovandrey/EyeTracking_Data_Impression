using DataImpression.AbstractMVVM;
using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TimeLineControlLibrary;

namespace DataImpression.ViewModel
{ 
  
    class FixationsTimelineVM: DocumentBodyVM
    {
        #region ctor
        public FixationsTimelineVM():base()
        {
            OnPropertyChanged("FixationsTimelineStepChartVM");

            OnPropertyChanged("Bars");
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model => Model.GetModel();
        #endregion


        #region Properties
        ObservableCollection<Bar> bars;
        public ObservableCollection<Bar> Bars
        {
            get
            {
                if (bars != null) return bars; 
                bars = new ObservableCollection<Bar>();
                foreach (var f in _model.Results.FAOIHitsOnTimeIntervalList)
                {
                    if (f.FAOIHits.Count>0)
                        bars.Add(ConvertFAOIHitsOnTimeIntervalToBar(f));
                }

                return bars;
            }
        }

        public TimeSpan FullTime
        {
            get { return _model.Results.FullTime.Value; }
        }

        #endregion


        #region Methods
        private Bar ConvertFAOIHitsOnTimeIntervalToBar(FAOIHitsOnTimeInterval f)
        {
         
            return new Bar(
                f.TimeInterval.TimeBegin,
                f.TimeInterval.TimeEnd,
                f.FAOIHits[0].Name,
                f.FAOIHits[0].OrderedNumber,
                f.FAOIHits[0].OrderedNumber * 15,
                ConvertOrderedNumberToBrush(f.FAOIHits[0].OrderedNumber),
                new SolidColorBrush(Colors.Black));


        }

        private Brush ConvertOrderedNumberToBrush(int orderedNumber)
        {
            switch (orderedNumber)
            {
                case 1: return new SolidColorBrush(Colors.Red);
                case 2: return new SolidColorBrush(Colors.Blue);
                case 3: return new SolidColorBrush(Colors.Green);
                case 4: return new SolidColorBrush(Colors.Yellow);
                case 5: return new SolidColorBrush(Colors.Cyan);
                case 6: return new SolidColorBrush(Colors.Purple);
                case 7: return new SolidColorBrush(Colors.Orange);
                case 8: return new SolidColorBrush(Colors.GreenYellow);
                case 9: return new SolidColorBrush(Colors.DarkGreen);
                case 10: return new SolidColorBrush(Colors.Violet);
                case 11: return new SolidColorBrush(Colors.Pink);
                case 12: return new SolidColorBrush(Colors.Brown);
                case 13: return new SolidColorBrush(Colors.Olive);

                default: return new SolidColorBrush(Colors.LightGray);
                   
            }

        }

        
        #endregion

        #region Commands

        

        #endregion

    }
}
