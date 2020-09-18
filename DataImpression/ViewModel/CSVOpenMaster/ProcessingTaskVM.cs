﻿using DataImpression.AbstractMVVM;
using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DataImpression.ViewModel
{
    public class ProcessingTaskVM : INPCBase
    {
        #region ctor
        public ProcessingTaskVM(Model model, CSVOpenMasterVM cSVOpenMasterVM)
        {
            _model = model;
            CSVOpenMasterVM = cSVOpenMasterVM;
        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;
        CSVOpenMasterVM CSVOpenMasterVM;

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

        private double progress;
        public double ProgressInPercents
        {
            get { return progress / 100; }
            set { OnPropertyChanged("ProgressInPercents"); progress = value * 100; }
        }

        private string stage;
        public string Stage
        {
            get { return stage; }
            set { OnPropertyChanged("Stage"); stage = value; }
        }
        #endregion
        #region Methods

        bool canExecuteNextInputStage = false;
        public bool CanExecuteNextInputStage()
        {
            return canExecuteNextInputStage;
        }

        private async void BeginProcessing()
        {
            RawDataProcessor rawDataProcessor = new RawDataProcessor(_model.SourceData, _model.Results);
            try
            {
                await Task.Run(() =>
                {
                    ProgressRefresh();
                    rawDataProcessor.ConvertCSVRawDataToFAOIHitsOnTimeIntervalList(ref progress, ref stage);
                    Application.Current.Dispatcher.Invoke(new Action(() => canExecuteNextInputStage = true));
                }
                );
            }

            catch (Exception e)
            {
                MessageBox.Show("Ошибка считывания файла " + _model.SourceData.CSVFileName +
                                Environment.NewLine + "Ошибка: " + e.Message+
                                Environment.NewLine + "StackTrace: " + e.StackTrace);

            }

            CSVOpenMasterVM.NextInputCommand.InvalidateRequerySuggested();
        }

        private async void ProgressRefresh()
        {
            await Task.Run(() =>
            {
                double p = 0;

                while (p < 100)
                {
                    Thread.Sleep(100);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        OnPropertyChanged("ProgressInPercents"); OnPropertyChanged("Stage");
                        p = progress;
                    }));
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    OnPropertyChanged("ProgressInPercents"); OnPropertyChanged("Stage");
                }));

            });
        }

        #endregion

        #region Commands

        private RelayCommand beginProcessingCommand;
        public RelayCommand BeginProcessingCommand
        {
            get
            {
                return beginProcessingCommand ?? (beginProcessingCommand = new RelayCommand(obj =>
                {

                    BeginProcessing();
                }));
            }
        }
        #endregion
    }
}
