using DataImpression.AbstractMVVM;
using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataImpression.ViewModel
{
    public class ProcessingTaskVM: INPCBase
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
        #endregion
        #region Methods
        public bool CanExecuteNextInputStage()
        {
            return true; //   if (FAOIsVM?.Count > 0) return true; else return false;
        }

        public void BeginProcessing()
        {
            RawDataProcessor rawDataProcessor = new RawDataProcessor(_model.SourceData, _model.Results);
            rawDataProcessor.ConvertCSVRawDataToFAOIHitsOnTimeIntervalList();
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
