using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.View.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataImpression.ViewModel
{
    public class ResultsViewAreaVM: INPCBase
    {
        #region ctor
        public ResultsViewAreaVM(Model model)
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

        public ObservableCollection<FileVM> ResultViews = new ObservableCollection<FileVM>();

        internal void ResultViewsAdd(string v)
        {
            ResultViews.Add(new FileVM());
            OnPropertyChanged("ResultViews");
            Test++; OnPropertyChanged("Test");

        }

        public int Test { get; private set; } = 3;
        #endregion
    }
}
