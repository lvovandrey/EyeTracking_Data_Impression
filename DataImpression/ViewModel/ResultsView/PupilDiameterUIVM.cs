using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PupilDiameterControlLibrary;

namespace DataImpression.ViewModel.ResultsView
{
    class PupilDiameterUIVM: DocumentBodyVM
    {
        #region ctor
        public PupilDiameterUIVM() : base()
        {
            OnPropertyChanged("PupilDiameter");

        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model => Model.GetModel();
        #endregion


        #region Properties
        ObservableCollection<TimeSpan_PupilDiameters_Pair> pupilDiameters;
        public ObservableCollection<TimeSpan_PupilDiameters_Pair> PupilDiameter
        {
            get
            {
                if (pupilDiameters != null) return pupilDiameters;
                pupilDiameters = new ObservableCollection<TimeSpan_PupilDiameters_Pair>();

                foreach (var item in _model.Results.PupilDiameter.Results)
                {
                    var value = new TimeSpan_PupilDiameters_Pair()
                    {
                        PupilDiameterLeft = item.Values[0],
                        PupilDiameterRight = item.Values[1],
                        Time = item.Time
                    };
                    pupilDiameters.Add(value);
                }
                return pupilDiameters;
            }
        }


        #endregion


        #region Methods



        #endregion

        #region Commands



        #endregion
    }
}
