using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public enum CSVFileOpenStage
    {
        None,
        TimeColumnChoice,
        AOIHitColumnsChoice,
        FAOIsInput,
        ProcessingTask,
        ViewResults
    }


    public class CSVOpenMasterVM
    {
        #region ctor
        public CSVOpenMasterVM(Model _model, MainWindow mainWindow)
        {
            TimeColumnChoiceVM = new TimeColumnChoiceVM(_model);
            AOIHitColumnsChoiceVM = new AOIHitColumnsChoiceVM(_model);
            FAOIsInputVM = new FAOIsInputVM(_model, MainWindow.FAOIsInput.FAOIsInputListView);
            ProcessingTaskVM = new ProcessingTaskVM(_model);

            InputStage = CSVFileOpenStage.None;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;

        MainWindow MainWindow;
        #endregion


        #region Properties
        TimeColumnChoiceVM timeColumnChoiceVM;
        public TimeColumnChoiceVM TimeColumnChoiceVM { get { return timeColumnChoiceVM; } set { timeColumnChoiceVM = value; OnPropertyChanged("TimeColumnChoiceVM"); } }

        AOIHitColumnsChoiceVM aOIHitColumnsChoiceVM;
        public AOIHitColumnsChoiceVM AOIHitColumnsChoiceVM { get { return aOIHitColumnsChoiceVM; } set { aOIHitColumnsChoiceVM = value; OnPropertyChanged("AOIHitColumnsChoiceVM"); } }

        FAOIsInputVM fAOIsInputVM;
        public FAOIsInputVM FAOIsInputVM { get { return fAOIsInputVM; } set { fAOIsInputVM = value; OnPropertyChanged("FAOIsInputVM"); } }

        ProcessingTaskVM processingTaskVM;
        public ProcessingTaskVM ProcessingTaskVM { get { return processingTaskVM; } set { processingTaskVM = value; OnPropertyChanged("ProcessingTaskVM"); } }


        private CSVFileOpenStage inputStage;
        public CSVFileOpenStage InputStage
        {
            get
            {
                return inputStage;
            }
            set
            {
                inputStage = value;
                OnPropertyChanged("InputStage");
                OnPropertyChanged("InputPageTitle");
                RefreshInputElementsVisibility();
            }
        }

        bool canExecuteNextInputStage = false;
        bool CanExecuteNextInputStage
        {
            get
            {
                switch (inputStage)
                {
                    case CSVFileOpenStage.None:
                        {
                            return false;
                            break;
                        }
                    case CSVFileOpenStage.TimeColumnChoice:
                        {
                            return TimeColumnChoiceVM.CanExecuteNextInputStage();
                            break;
                        }
                    case CSVFileOpenStage.AOIHitColumnsChoice:
                        {
                            return AOIHitColumnsChoiceVM.CanExecuteNextInputStage();
                            break;
                        }
                    case CSVFileOpenStage.FAOIsInput:
                        {
                            return FAOIsInputVM.CanExecuteNextInputStage();
                            break;
                        }
                    case CSVFileOpenStage.ProcessingTask:
                        {
                            return ProcessingTaskVM.CanExecuteNextInputStage();
                            break;
                        }
                    case CSVFileOpenStage.ViewResults:
                        {
                            return ResultsViewAreaVM.CanExecuteNextInputStage();
                            break;
                        }
                    default:
                        {
                            return false;
                            break;
                        }
                }
            }
        }

        public string InputPageTitle
        {
            get
            {
                switch (inputStage)
                {
                    case CSVFileOpenStage.None:
                        {
                            return "";
                            break;
                        }
                    case CSVFileOpenStage.TimeColumnChoice:
                        {
                            return "Выбор колонки csv-файла с временем";
                            break;
                        }
                    case CSVFileOpenStage.AOIHitColumnsChoice:
                        {
                            return "Выбор колонки csv-файла с попаданиями маркера взгляда в размеченные зоны (AOI Hit)";
                            break;
                        }
                    case CSVFileOpenStage.FAOIsInput:
                        {
                            return "Ввод перечня функциональных зон (FAOI)";
                            break;
                        }
                    case CSVFileOpenStage.ProcessingTask:
                        {
                            return "Обработка введенных данных, построение внутренней модели";
                            break;
                        }
                    case CSVFileOpenStage.ViewResults:
                        {
                            return "Отображение результатов";
                            break;
                        }
                    default:
                        {
                            return "";
                            break;
                        }
                }
            }
        }

        #endregion
    }
}
