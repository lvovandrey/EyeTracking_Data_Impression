using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.Models.Helpers;
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
            ResultsViewAreaVM = new ResultsViewAreaVM(CurrentProject, _model, this);
            ResultsViewAreaVM.Visibility = Visibility.Visible;

        }
        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model
        {
            get { return CurrentProject.Model; }
            set { CurrentProject.Model = value; }
        }

        Project CurrentProject = new Project();

        public Model GetModel() { return _model; }

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


        private RelayCommand newProjectCommand;
        public RelayCommand NewProjectCommand
        {
            get
            {
                return newProjectCommand ?? (newProjectCommand = new RelayCommand(obj =>
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Etprj-flie(*.Etprj)|*.Etprj";
                    saveFileDialog.Title = "Создайте файл проекта";
                    bool? res = saveFileDialog.ShowDialog();
                    if (res == null || res == false) return;
                    string filename = saveFileDialog.FileName;
                    CurrentProject = new Project(new Model(), filename);
                    ModelSerializer.SaveToXML(CurrentProject, CurrentProject.FilePath);
                    ResultsViewAreaVM = new ResultsViewAreaVM(CurrentProject, _model, this);
                }));
            }
        }


        private RelayCommand saveProjectCommand;
        public RelayCommand SaveProjectCommand
        {
            get
            {
                return saveProjectCommand ?? (saveProjectCommand = new RelayCommand(obj =>
                {
                    ModelSerializer.SaveToXML(CurrentProject, CurrentProject.FilePath);
                    ResultsViewAreaVM = new ResultsViewAreaVM(CurrentProject, _model, this);
                }));
            }
        }

        private RelayCommand openProjectCommand;
        public RelayCommand OpenProjectCommand
        {
            get
            {
                return openProjectCommand ?? (openProjectCommand = new RelayCommand(obj =>
                {
                    ModelSerializer.LoadFromXML(out CurrentProject);
                    ResultsViewAreaVM = new ResultsViewAreaVM(CurrentProject, _model, this);
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
