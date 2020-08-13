using DataImpression.AbstractMVVM;
using DataImpression.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;

namespace DataImpression.ViewModel
{
    public enum InputStage
    {
        None,
        TimeColumnChoice,
        AOIHitColumnsChoice,
        FAOIsInput,
        ProcessingTask, 
        ViewResults
    }
    public class MainWindowViewModel:INPCBase
    {

        #region ctor
        public MainWindowViewModel(Model model, MainWindow mainWindow)
        {
            _model = model;
            MainWindow = mainWindow;
            TimeColumnChoiceVM = new TimeColumnChoiceVM(_model);
            AOIHitColumnsChoiceVM = new AOIHitColumnsChoiceVM(_model);
            FAOIsInputVM = new FAOIsInputVM(_model, MainWindow.FAOIsInput.FAOIsInputListView);
            ProcessingTaskVM = new ProcessingTaskVM(_model);
          //  FAOIDiagramVM = new FAOIDiagramVM(_model);
            ResultsViewAreaVM = new ResultsViewAreaVM(_model);
            InputStage = InputStage.None;
            OnPropertyChanged("TimeColumnChoiceOpacity");
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

        //FAOIDiagramVM fAOIDiagramVM;
        //public FAOIDiagramVM FAOIDiagramVM { get { return fAOIDiagramVM; } set { fAOIDiagramVM = value; OnPropertyChanged("FAOIDiagramVM"); } }

        ResultsViewAreaVM resultsViewAreaVM;
        public ResultsViewAreaVM ResultsViewAreaVM { get { return resultsViewAreaVM; } set { resultsViewAreaVM = value; OnPropertyChanged("ResultsViewAreaVM"); } }


        private InputStage inputStage;
        public InputStage InputStage
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
                    case InputStage.None:
                        {
                            return false;
                            break;
                        }
                    case InputStage.TimeColumnChoice:
                        {
                            return TimeColumnChoiceVM.CanExecuteNextInputStage();
                            break;
                        }
                    case InputStage.AOIHitColumnsChoice:
                        {
                            return AOIHitColumnsChoiceVM.CanExecuteNextInputStage();
                            break;
                        }
                    case InputStage.FAOIsInput:
                        {
                            return FAOIsInputVM.CanExecuteNextInputStage();
                            break;
                        }
                    case InputStage.ProcessingTask:
                        {
                            return ProcessingTaskVM.CanExecuteNextInputStage();
                            break;
                        }
                    case InputStage.ViewResults:
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
                    case InputStage.None:
                        {
                            return "";
                            break;
                        }
                    case InputStage.TimeColumnChoice:
                        {
                            return "Выбор колонки csv-файла с временем";
                            break;
                        }
                    case InputStage.AOIHitColumnsChoice:
                        {
                            return "Выбор колонки csv-файла с попаданиями маркера взгляда в размеченные зоны (AOI Hit)";
                            break;
                        }
                    case InputStage.FAOIsInput:
                        {
                            return "Ввод перечня функциональных зон (FAOI)";
                            break;
                        }
                    case InputStage.ProcessingTask:
                        {
                            return "Обработка введенных данных, построение внутренней модели";
                            break;
                        }
                    case InputStage.ViewResults:
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

        #region Methods
        /// <summary>
        /// Метод для команды OpenCSVFileCommand - открывает csv файл и готовится к работе с ним (заполняет CSVCaption и CSVFileName в SourceData в модели). 
        /// </summary>
        public void OpenCSVFile()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog()==false) return;

            InputStage = InputStage.TimeColumnChoice;
            _model.SourceData.CSVFileName = openFileDialog.FileName;// файлнейм в модель  закидываем
            try
            { 
                List<string> caption_string = new CSVReader().TobiiCSVReadStrings(_model.SourceData.CSVFileName, 1);//читаем первую строку и далее ее разбиваем.
                List<string> splitted_caption_string = new List<string>(caption_string[0].Split('\t'));//разбиваем ее

                _model.SourceData.CSVCaption = Column.ToColumns(splitted_caption_string);//и преобразовываем в набор колонок и закидываем в модель
            }
            catch
            {
                InputStage = InputStage.None;
                MessageBox.Show("Не удалось считать заголовок csv-файла. Попробуйте открыть файл вручную и убедиться в правильности его формата.");
            }
        }


       
        void SwithToNextInputStage()
        {
            switch (inputStage)
            {
                case InputStage.None:
                    {
                        InputStage = InputStage.TimeColumnChoice;
                        break;
                    }
                case InputStage.TimeColumnChoice:
                    {
                        InputStage = InputStage.AOIHitColumnsChoice;
                        AOIHitColumnsChoiceVM = new AOIHitColumnsChoiceVM(_model);
                        break;
                    }
                case InputStage.AOIHitColumnsChoice:
                    {
                        InputStage = InputStage.FAOIsInput;
                        FAOIsInputVM = new FAOIsInputVM(_model, MainWindow.FAOIsInput.FAOIsInputListView);
                        break;
                    }
                case InputStage.FAOIsInput:
                    {
                        if (!FAOIsInputVM.RecordResultsToModel()) return; //TODO: вот это тоже плохо - тут он нужен этот метод, а в других местах его нет. А если забуду?
                        InputStage = InputStage.ProcessingTask;
                        break;
                    }
                case InputStage.ProcessingTask:
                    {
                        InputStage = InputStage.ViewResults;
                        ResultsViewAreaVM = new ResultsViewAreaVM(_model);
                        break;
                    }
                case InputStage.ViewResults:
                    {
                        InputStage = InputStage.None;
                        break;
                    }
                default:
                    {
                        InputStage = InputStage.None;
                        break;
                    }
            }

            RefreshInputElementsVisibility();
        }

        void RefreshInputElementsVisibility()
        {
            if(TimeColumnChoiceVM!=null)
                if (inputStage==InputStage.TimeColumnChoice) TimeColumnChoiceVM.Visibility = Visibility.Visible;
                else  TimeColumnChoiceVM.Visibility = Visibility.Collapsed;

            if (AOIHitColumnsChoiceVM != null)
                if (inputStage == InputStage.AOIHitColumnsChoice) AOIHitColumnsChoiceVM.Visibility = Visibility.Visible;
                else AOIHitColumnsChoiceVM.Visibility = Visibility.Collapsed;

            if (FAOIsInputVM != null)
                if (inputStage == InputStage.FAOIsInput) FAOIsInputVM.Visibility = Visibility.Visible;
                else FAOIsInputVM.Visibility = Visibility.Collapsed;

            if (ProcessingTaskVM != null)
                if (inputStage == InputStage.ProcessingTask) ProcessingTaskVM.Visibility = Visibility.Visible;
                else ProcessingTaskVM.Visibility = Visibility.Collapsed;

            if (ResultsViewAreaVM != null)
                if (inputStage == InputStage.ViewResults) ResultsViewAreaVM.Visibility = Visibility.Visible;
                else ResultsViewAreaVM.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region Commands


        private RelayCommand openCSVFileCommand;
        public RelayCommand OpenCSVFileCommand
        {
            get
            {
                return openCSVFileCommand ?? (openCSVFileCommand = new RelayCommand(obj =>
                {
                    OpenCSVFile();
                    TimeColumnChoiceVM = new TimeColumnChoiceVM(_model);
                }));
            }
        }



        private RelayCommand nextInputCommand;
        public RelayCommand NextInputCommand
        {
            get
            {
                return nextInputCommand ?? (nextInputCommand = new RelayCommand(obj =>
                {
                    SwithToNextInputStage();
                },
                (obj) =>  CanExecuteNextInputStage == true 
                ));
            }
        }

        private RelayCommand addResultsViewCommand;
        public RelayCommand AddResultsViewCommand
        {
            get
            {
                return addResultsViewCommand ?? (addResultsViewCommand = new RelayCommand(obj =>
                {
                    ResultsViewAreaVM.ResultViewsAdd("Новое что-то");
                }));
            }
        }



        #endregion
    }
}
