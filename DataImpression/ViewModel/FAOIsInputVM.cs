using DataImpression.AbstractMVVM;
using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataImpression.ViewModel
{
     public class FAOIsInputVM : INPCBase
    {
        #region ctor
        public FAOIsInputVM(Model model)
        {
            _model = model;
            OnPropertyChanged("FAOIsVM");
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;
        #endregion


        #region Properties
        ObservableCollection<FAOI> FAOIstmp;
        ObservableCollection<FAOI> FAOIsVM
        {
            get
            {
                if (FAOIstmp != null)
                {
                    return FAOIstmp;
                }
                FAOIstmp = new ObservableCollection<FAOI>();
                return FAOIstmp;
            }
        }




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
        #region Methods
        public bool CanExecuteNextInputStage()
        {
            return false;
            // if (GetCSVTimeColumn()?.Name != null) return true; else return false; //Не хочу я тернарный оператор
        }
        #endregion

        #region Commands

        #endregion
    }
}
