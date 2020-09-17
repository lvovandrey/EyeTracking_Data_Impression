using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.View;
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
            ResultsViewAreaVM.Visibility = Visibility.Visible;

        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;

        MainWindow MainWindow;
        CSVOpenMasterView CSVOpenMasterView;
        #endregion


        #region Properties
        
        ResultsViewAreaVM resultsViewAreaVM;
        public ResultsViewAreaVM ResultsViewAreaVM { get { return resultsViewAreaVM; } set { resultsViewAreaVM = value; OnPropertyChanged("ResultsViewAreaVM"); } }

        CSVOpenMasterVM cSVOpenMasterVM;
        public CSVOpenMasterVM CSVOpenMasterVM { get { return cSVOpenMasterVM; } set { cSVOpenMasterVM = value; OnPropertyChanged("CSVOpenMasterVM"); } }

        


        #endregion



        #region Commands


        private RelayCommand openCSVFileCommand;
        public RelayCommand OpenCSVFileCommand
        {
            get
            {
                return openCSVFileCommand ?? (openCSVFileCommand = new RelayCommand(obj =>
                {

                    CSVOpenMasterView = new CSVOpenMasterView();
                    CSVOpenMasterVM = new CSVOpenMasterVM(_model, CSVOpenMasterView);
                    CSVOpenMasterView.DataContext = this.CSVOpenMasterVM;
                    CSVOpenMasterVM.OpenCSVFile();
                    
                }));
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
