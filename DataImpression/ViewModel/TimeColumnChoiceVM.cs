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
    public delegate void AcitonColumnAndCheckVMArgument(ColumnAndCheckVM newColumnAndCheckVM);

    public class TimeColumnChoiceVM : INPCBase
    {
        #region ctor
        public TimeColumnChoiceVM(Model model)
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
                _model.SourceData.CSVTimeColumn = GetCSVTimeColumn();
                return columnstmp;
            }
            //set { }
        }

        void OnCheckColumn(ColumnAndCheckVM newColumnAndCheckVM)
        {
            foreach (var c in columnsVM)
                c.Check(false);

            foreach (var c in columnsVM)
                if (c == newColumnAndCheckVM)
                    c.Check(true);

            _model.SourceData.CSVTimeColumn = GetCSVTimeColumn();
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
        Column GetCSVTimeColumn()
        {
            Column column = null;
            foreach (var c in ColumnsVM)
                if(c.IsChecked) { column = c.Column; break; }
            return column;
        }


        public bool CanExecuteNextInputStage()
        {   
            if (GetCSVTimeColumn()?.Name != null) return true; else return false; //Не хочу я тернарный оператор
        }
        #endregion

        #region Commands

        #endregion
    }

    [Serializable]
    public class ColumnAndCheckVM : INPCBase
    {
        public ColumnAndCheckVM(Column _column, bool _isChecked, AcitonColumnAndCheckVMArgument _checkColumn)
        {
            Column = _column;
            IsChecked = _isChecked;
            checkColumn = _checkColumn;
        }
        bool isChecked;
        AcitonColumnAndCheckVMArgument checkColumn;
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; OnPropertyChanged("IsChecked"); checkColumn?.Invoke(this); }
        }

        public void Check(bool ch)
        {
            isChecked = ch;
            OnPropertyChanged("IsChecked");
        }

        public Column Column;
        public string Name
        {
            get { return Column.Name; }
        }
        public int OrderedNumber
        {
            get { return Column.OrderedNumber; }
        }
    }
}

