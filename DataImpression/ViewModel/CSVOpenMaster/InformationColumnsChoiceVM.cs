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
    public class InformationColumnsChoiceVM : INPCBase
    {
        static Random random = new Random();
        #region ctor
        public InformationColumnsChoiceVM()
        {
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model => Model.GetModel();
        #endregion




        #region Properties



        ObservableCollection<Column> columnsVM;
        public ObservableCollection<Column> ColumnsVM
        {
            get
            {
                if (columnsVM == null || columnsVM.Count == 0) InitColumnsVMAndProperties();
                return columnsVM;
            }
        }


        


        private Column participantNameColumn;
        public Column ParticipantNameColumn
        {
            get { return participantNameColumn; }
            set
            {
                participantNameColumn = value;
                OnPropertyChanged("ParticipantNameColumn");
            }
        }

        private Column recordingNameColumn;
        public Column RecordingNameColumn
        {
            get { return recordingNameColumn; }
            set
            {
                recordingNameColumn = value;
                OnPropertyChanged("RecordingNameColumn");
            }
        }


        private Column recordingDateColumn;
        public Column RecordingDateColumn
        {
            get { return recordingDateColumn; }
            set
            {
                recordingDateColumn = value;
                OnPropertyChanged("RecordingDateColumn");
            }
        }


        private Column recordingStartTimeColumn;
        public Column RecordingStartTimeColumn
        {
            get { return recordingStartTimeColumn; }
            set
            {
                recordingStartTimeColumn = value;
                OnPropertyChanged("RecordingStartTimeColumn");
            }
        }

        private Column pupilDiameterRightColumn;
        public Column PupilDiameterRightColumn
        {
            get { return pupilDiameterRightColumn; }
            set
            {
                pupilDiameterRightColumn = value;
                OnPropertyChanged("PupilDiameterRightColumn");
            }
        }

        private Column pupilDiameterLeftColumn;
        public Column PupilDiameterLeftColumn
        {
            get { return pupilDiameterLeftColumn; }
            set
            {
                pupilDiameterLeftColumn = value;
                OnPropertyChanged("PupilDiameterLeftColumn");
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
                var oldVisibility = visibility;
                visibility = value;
                OnPropertyChanged("Visibility");
                if (oldVisibility != visibility && visibility == Visibility.Visible)
                {
                    InitColumnsVMAndProperties();
                    RaiseAllPropertyChanged();
                }
            }
        }
        #endregion
        #region Methods
        public void RaiseAllPropertyChanged()
        {
            OnPropertyChanged("ColumnsVM");
            OnPropertyChanged("ParticipantNameColumn");
            OnPropertyChanged("RecordingNameColumn");
            OnPropertyChanged("RecordingDateColumn");
            OnPropertyChanged("RecordingStartTimeColumn");
            OnPropertyChanged("PupilDiameterRightColumn");
            OnPropertyChanged("PupilDiameterLeftColumn");
        }

        public bool CanExecuteNextInputStage()
        {
            return true;
        }


        private void InitColumnsVMAndProperties()
        {
            columnsVM = new ObservableCollection<Column>();
            columnsVM.Add(new Column(-1, "--В файле нет этой колонки--"));
            foreach (var _column in _model.SourceData.CSVCaption) columnsVM.Add(_column);

            foreach (var col in columnsVM)
            {
                switch (col.Name)
                {
                    case "Participant name": ParticipantNameColumn = col; break;
                    case "Recording name": RecordingNameColumn = col; break;
                    case "Recording date": RecordingDateColumn = col; break;
                    case "Recording start time": RecordingStartTimeColumn = col; break;
                    case "Pupil diameter right": PupilDiameterRightColumn = col; break;
                    case "Pupil diameter left": PupilDiameterLeftColumn = col; break;

                    default: break;
                }

            }
        }

        internal bool RecordResultsToModel()
        {
            _model.SourceData.OptionalDataCSVColumns.Add("Имя испытуемого", ParticipantNameColumn);
            _model.SourceData.OptionalDataCSVColumns.Add("Имя записи", RecordingNameColumn);
            _model.SourceData.OptionalDataCSVColumns.Add("Дата записи", RecordingDateColumn);
            _model.SourceData.OptionalDataCSVColumns.Add("Время записи", RecordingStartTimeColumn);
            _model.SourceData.OptionalDataCSVColumns.Add("Pupil diameter right", PupilDiameterRightColumn);
            _model.SourceData.OptionalDataCSVColumns.Add("Pupil diameter left", PupilDiameterLeftColumn);

            return true;
        }
        #endregion

        #region Commands
        private RelayCommand setDefaultCommand;
        public RelayCommand SetDefaultCommand
        {
            get
            {
                return setDefaultCommand ?? (setDefaultCommand = new RelayCommand(obj =>
                {
                    InitColumnsVMAndProperties();
                }));
            }
        }


        #endregion
    }
}
