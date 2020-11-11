using DataImpression.AbstractMVVM;
using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DataImpression.ViewModel
{
    public class AOIHitColumnsChoiceVM : INPCBase
    {
        #region ctor
        public AOIHitColumnsChoiceVM()
        {
            OnPropertyChanged("ColumnsVM");
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model model => Model.GetModel();
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
                foreach (var _column in model.SourceData.CSVCaption)
                {
                    bool _isChecked = false;
                    if (_column.Name.Contains("AOI hit")) _isChecked = true;
                    var cc = new ColumnAndCheckVM(_column, _isChecked, OnCheckColumn);
                    columnstmp.Add(cc);
                }
                model.SourceData.CSVAOIHitsColumns = GetCSVAOIHitsColumns();
                return columnstmp;
            }
            //set { }
        }

        void OnCheckColumn(ColumnAndCheckVM newColumnAndCheckVM)
        {
            model.SourceData.CSVAOIHitsColumns = GetCSVAOIHitsColumns();
        }

        private List<Column> GetCSVAOIHitsColumns()
        {
            List<Column> columns = new List<Column>();
            foreach (var c in columnsVM)
                if (c.IsChecked)
                    columns.Add(c.Column);
            return columns;
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
           // return false;
           if (GetCSVAOIHitsColumns()?.Count>0) return true; else return false; //Не хочу я тернарный оператор
        }
        #endregion

        #region Commands

        #endregion
    }
}