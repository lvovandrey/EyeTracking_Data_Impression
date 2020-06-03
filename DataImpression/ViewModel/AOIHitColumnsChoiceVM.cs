using DataImpression.AbstractMVVM;
using DataImpression.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataImpression.ViewModel
{
    public class AOIHitColumnsChoiceVM : INPCBase
    {
        #region ctor
        public AOIHitColumnsChoiceVM(Model model)
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
        ObservableCollection<ColumnAndCheckVM> columnstmp;
        ObservableCollection<ColumnAndCheckVM> columnsVM
        {
            get
            {
                if (columnstmp != null)
                {
                    return columnstmp;
                }
                columnstmp = new ObservableCollection<ColumnAndCheckVM>();
                foreach (var _column in _model.SourceData.CSVCaption)
                {
                    bool _isChecked = false;
                    if (_column.Name == "Recording timestamp") _isChecked = true;
                    var cc = new ColumnAndCheckVM(_column, _isChecked, OnCheckColumn);
                    columnstmp.Add(cc);
                }
                return columnstmp;
            }
            //set { }
        }

        void OnCheckColumn(ColumnAndCheckVM newColumnAndCheckVM)
        {

        }

        public ObservableCollection<ColumnAndCheckVM> ColumnsVM
        {
            get { return columnsVM; }
            //set { columnsVM = value; }
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