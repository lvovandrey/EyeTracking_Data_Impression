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
    public class InformationColumnsChoiceVM  : INPCBase
    {
        static Random random = new Random();
        #region ctor
        public InformationColumnsChoiceVM(Model model)
        {
            _model = model;
            OnPropertyChanged("TEST");
            TEST = random.Next(1, 100000);
            SetDefaultParticipantNameColumn();
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;
        #endregion




        #region Properties
        int test;
        public int TEST
        { get { return test; }
            set { test = value; OnPropertyChanged("TEST"); }
        }

        public void RaiseAllPropertyChanged()
        {
            OnPropertyChanged("TEST");
            OnPropertyChanged("ColumnsVM");
            OnPropertyChanged("ParticipantNameColumn");
        }

        internal void RaiseOnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }

        ObservableCollection<ColumnAndCheckVM> columnstmp;
        ObservableCollection<ColumnAndCheckVM> columnsVM
        {
            get
            {
                columnstmp = new ObservableCollection<ColumnAndCheckVM>();
                foreach (var _column in _model.SourceData.CSVCaption)
                {
                    bool _isChecked = false;
                    var cc = new ColumnAndCheckVM(_column, _isChecked,(a)=> { });
                    columnstmp.Add(cc);
                }
                
                return columnstmp;
            }
        }

        public ObservableCollection<ColumnAndCheckVM> ColumnsVM
        {
            get { return columnsVM; }
        }


        private ColumnAndCheckVM participantNameColumn;
        public ColumnAndCheckVM ParticipantNameColumn
        {
            get { return participantNameColumn; }
            set { participantNameColumn = value; OnPropertyChanged("ParticipantNameColumn"); }

        }
        public void SetDefaultParticipantNameColumn()
        {
            if (TEST > 0) ;
            if (ParticipantNameColumn == null && ColumnsVM.Count>0)
                ParticipantNameColumn = ColumnsVM[0];
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
                RaiseAllPropertyChanged();
                visibility = value;
                OnPropertyChanged("Visibility");
                RaiseAllPropertyChanged();
            }
        }
        #endregion  
        #region Methods

        public bool CanExecuteNextInputStage()
        {
            return true; 
        }
        #endregion

        #region Commands

        #endregion
    }
}
