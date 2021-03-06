﻿using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.View;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataImpression.ViewModel
{
    public enum CSVFileOpenStage
    {
        None,
        TimeColumnChoice,
        InformationColumnsChoice,
        AOIHitColumnsChoice,
        FAOIsInput,
        ProcessingTask,
        ViewResults
    }


    public class CSVOpenMasterVM: INPCBase
    {
        #region ctor
        public CSVOpenMasterVM(CSVOpenMasterView cSVOpenMasterView)
        {
            
            CSVOpenMasterView = cSVOpenMasterView;

            TimeColumnChoiceVM = new TimeColumnChoiceVM();
            InformationColumnsChoiceVM = new InformationColumnsChoiceVM();
            AOIHitColumnsChoiceVM = new AOIHitColumnsChoiceVM();
            FAOIsInputVM = new FAOIsInputVM(CSVOpenMasterView.FAOIsInput.FAOIsInputListView);
            ProcessingTaskVM = new ProcessingTaskVM(this);

            InputStage = CSVFileOpenStage.None;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model model => Model.GetModel();

        CSVOpenMasterView CSVOpenMasterView;
        #endregion


        #region Properties
        TimeColumnChoiceVM timeColumnChoiceVM;
        public TimeColumnChoiceVM TimeColumnChoiceVM { get { return timeColumnChoiceVM; } set { timeColumnChoiceVM = value; OnPropertyChanged("TimeColumnChoiceVM"); } }

        InformationColumnsChoiceVM informationColumnsChoiceVM;
        public InformationColumnsChoiceVM InformationColumnsChoiceVM { get { return informationColumnsChoiceVM; } set { informationColumnsChoiceVM = value; OnPropertyChanged("InformationColumnsChoiceVM "); } }


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
                    case CSVFileOpenStage.InformationColumnsChoice:
                        {
                            return InformationColumnsChoiceVM.CanExecuteNextInputStage();
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
                            return true;
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
                    case CSVFileOpenStage.InformationColumnsChoice:
                        {
                            return "Выбор колонок с дополнительной информацией";
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

        #region Methods
        /// <summary>
        /// Метод для команды OpenCSVFileCommand - открывает csv файл и готовится к работе с ним (заполняет CSVCaption и CSVFileName в SourceData в модели). 
        /// </summary>
        public void OpenCSVFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == false) return;

            if (model.HaveData)
            {
                var res = MessageBox.Show("В проекте уже есть данные. Стереть их и загрузить данные из выбранного файла?",
                                          "Замена данных в проекте", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (res == MessageBoxResult.Cancel) return;
            }

            Model.ClearModel();
            model.HaveData = true;

            CSVOpenMasterView.Show();
            
            InputStage = CSVFileOpenStage.TimeColumnChoice;
            model.SourceData.CSVFileName = openFileDialog.FileName;// файлнейм в модель  закидываем
            try
            {
                List<string> caption_string = new CSVReader().TobiiCSVReadStrings(model.SourceData.CSVFileName, 1);//читаем первую строку и далее ее разбиваем.
                List<string> splitted_caption_string = new List<string>(caption_string[0].Split('\t'));//разбиваем ее

                model.SourceData.CSVCaption = Column.ToColumns(splitted_caption_string);//и преобразовываем в набор колонок и закидываем в модель
            }
            catch
            {
                InputStage = CSVFileOpenStage.None;
                MessageBox.Show("Не удалось считать заголовок csv-файла. Попробуйте открыть файл вручную и убедиться в правильности его формата.");
            }
            TimeColumnChoiceVM = new TimeColumnChoiceVM();
            OnPropertyChanged("TimeColumnChoiceVM");
            
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
                        InputStage = CSVFileOpenStage.InformationColumnsChoice;
                        break;
                    }

                case CSVFileOpenStage.InformationColumnsChoice:
                    {
                        if (!InformationColumnsChoiceVM.RecordResultsToModel()) return;
                        InputStage = CSVFileOpenStage.AOIHitColumnsChoice;
                        AOIHitColumnsChoiceVM = new AOIHitColumnsChoiceVM();
                        break;
                    }
                case CSVFileOpenStage.AOIHitColumnsChoice:
                    {
                        InputStage = CSVFileOpenStage.FAOIsInput;
                        FAOIsInputVM = new FAOIsInputVM(CSVOpenMasterView.FAOIsInput.FAOIsInputListView);
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
                        CSVOpenMasterView.Close();
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

            if (InformationColumnsChoiceVM != null)
                if (inputStage == CSVFileOpenStage.InformationColumnsChoice) InformationColumnsChoiceVM.Visibility = Visibility.Visible;
                else InformationColumnsChoiceVM.Visibility = Visibility.Collapsed;

            if (AOIHitColumnsChoiceVM != null)
                if (inputStage == CSVFileOpenStage.AOIHitColumnsChoice) AOIHitColumnsChoiceVM.Visibility = Visibility.Visible;
                else AOIHitColumnsChoiceVM.Visibility = Visibility.Collapsed;

            if (FAOIsInputVM != null)
                if (inputStage == CSVFileOpenStage.FAOIsInput) FAOIsInputVM.Visibility = Visibility.Visible;
                else FAOIsInputVM.Visibility = Visibility.Collapsed;

            if (ProcessingTaskVM != null)
                if (inputStage == CSVFileOpenStage.ProcessingTask) ProcessingTaskVM.Visibility = Visibility.Visible;
                else ProcessingTaskVM.Visibility = Visibility.Collapsed;


        }
        #endregion

        #region Commands
        private RelayCommand nextInputCommand;
        public RelayCommand NextInputCommand
        {
            get
            {
                return nextInputCommand ?? (nextInputCommand = new RelayCommand(obj =>
                {
                    SwithToNextInputStage();
                },
                (obj) => 
                CanExecuteNextInputStage == true
                ));
            }
        }
        #endregion
    }
}
