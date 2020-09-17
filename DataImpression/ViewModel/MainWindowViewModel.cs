using DataImpression.AbstractMVVM;
using DataImpression.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;

namespace DataImpression.ViewModel
{

    public class MainWindowViewModel : INPCBase
    {

        #region ctor
        public MainWindowViewModel(Model model, MainWindow mainWindow)
        {
            _model = model;
            MainWindow = mainWindow;

            //  FAOIDiagramVM = new FAOIDiagramVM(_model);
            ResultsViewAreaVM = new ResultsViewAreaVM(_model, this);
          

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
        
        ResultsViewAreaVM resultsViewAreaVM;
        public ResultsViewAreaVM ResultsViewAreaVM { get { return resultsViewAreaVM; } set { resultsViewAreaVM = value; OnPropertyChanged("ResultsViewAreaVM"); } }



        #endregion

        #region Methods
        /// <summary>
        /// Метод для команды OpenCSVFileCommand - открывает csv файл и готовится к работе с ним (заполняет CSVCaption и CSVFileName в SourceData в модели). 
        /// </summary>
        public void OpenCSVFile()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false) return;

            InputStage = CSVFileOpenStage.TimeColumnChoice;
            _model.SourceData.CSVFileName = openFileDialog.FileName;// файлнейм в модель  закидываем
            try
            {
                List<string> caption_string = new CSVReader().TobiiCSVReadStrings(_model.SourceData.CSVFileName, 1);//читаем первую строку и далее ее разбиваем.
                List<string> splitted_caption_string = new List<string>(caption_string[0].Split('\t'));//разбиваем ее

                _model.SourceData.CSVCaption = Column.ToColumns(splitted_caption_string);//и преобразовываем в набор колонок и закидываем в модель
            }
            catch
            {
                InputStage = CSVFileOpenStage.None;
                MessageBox.Show("Не удалось считать заголовок csv-файла. Попробуйте открыть файл вручную и убедиться в правильности его формата.");
            }
        }



        void SwithToNextInputStage()
        {
            switch (inputStage)
            {
                case CSVFileOpenStage.None:
                    {
                        InputStage = CSVFileOpenStage.TimeColumnChoice;
                        break;
                    }
                case CSVFileOpenStage.TimeColumnChoice:
                    {
                        InputStage = CSVFileOpenStage.AOIHitColumnsChoice;
                        AOIHitColumnsChoiceVM = new AOIHitColumnsChoiceVM(_model);
                        break;
                    }
                case CSVFileOpenStage.AOIHitColumnsChoice:
                    {
                        InputStage = CSVFileOpenStage.FAOIsInput;
                        FAOIsInputVM = new FAOIsInputVM(_model, MainWindow.FAOIsInput.FAOIsInputListView);
                        break;
                    }
                case CSVFileOpenStage.FAOIsInput:
                    {
                        if (!FAOIsInputVM.RecordResultsToModel()) return; //TODO: вот это тоже плохо - тут он нужен этот метод, а в других местах его нет. А если забуду?
                        InputStage = CSVFileOpenStage.ProcessingTask;
                        break;
                    }
                case CSVFileOpenStage.ProcessingTask:
                    {
                        InputStage = CSVFileOpenStage.ViewResults;
                        ResultsViewAreaVM = new ResultsViewAreaVM(_model, this);
                        break;
                    }
                case CSVFileOpenStage.ViewResults:
                    {
                        InputStage = CSVFileOpenStage.None;
                        break;
                    }
                default:
                    {
                        InputStage = CSVFileOpenStage.None;
                        break;
                    }
            }

            RefreshInputElementsVisibility();
        }

        void RefreshInputElementsVisibility()
        {
            if (TimeColumnChoiceVM != null)
                if (inputStage == CSVFileOpenStage.TimeColumnChoice) TimeColumnChoiceVM.Visibility = Visibility.Visible;
                else TimeColumnChoiceVM.Visibility = Visibility.Collapsed;

            if (AOIHitColumnsChoiceVM != null)
                if (inputStage == CSVFileOpenStage.AOIHitColumnsChoice) AOIHitColumnsChoiceVM.Visibility = Visibility.Visible;
                else AOIHitColumnsChoiceVM.Visibility = Visibility.Collapsed;

            if (FAOIsInputVM != null)
                if (inputStage == CSVFileOpenStage.FAOIsInput) FAOIsInputVM.Visibility = Visibility.Visible;
                else FAOIsInputVM.Visibility = Visibility.Collapsed;

            if (ProcessingTaskVM != null)
                if (inputStage == CSVFileOpenStage.ProcessingTask) ProcessingTaskVM.Visibility = Visibility.Visible;
                else ProcessingTaskVM.Visibility = Visibility.Collapsed;

            if (ResultsViewAreaVM != null)
                if (inputStage == CSVFileOpenStage.ViewResults) ResultsViewAreaVM.Visibility = Visibility.Visible;
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
                (obj) => CanExecuteNextInputStage == true
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
                    if (obj != null)
                        ResultsViewAreaVM.ResultViewsAdd(obj.ToString());

                }));
            }
        }




        #endregion
    }
}
