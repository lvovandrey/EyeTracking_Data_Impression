using DataImpression.AbstractMVVM;
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
        public ProcessingTaskVM(Model model)
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
        public double Progress
        {
            get { return progress; }
            set { OnPropertyChanged("Progress"); progress = value; }
        }

        private string stage;
        public string Stage
        {
            get { return stage; }
            set { OnPropertyChanged("Progress"); stage = value; }
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
            await Task.Run(() =>
            {
                ProgressRefresh();
                rawDataProcessor.ConvertCSVRawDataToFAOIHitsOnTimeIntervalList(ref progress, ref stage);

                Application.Current.Dispatcher.Invoke(new Action(() => canExecuteNextInputStage = true));
            }
            );
        }

        private async void ProgressRefresh()
        {
            await Task.Run(() =>
            {
                double p=0;

                while (p < 100)
                {
                    Thread.Sleep(100);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        OnPropertyChanged("Progress");
                        p = progress;
                    }));
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    OnPropertyChanged("Progress");
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
