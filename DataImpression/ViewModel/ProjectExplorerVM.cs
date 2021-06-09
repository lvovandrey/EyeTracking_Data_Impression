using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.ViewModel.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public class ProjectExplorerVM: ToolVM
    {
        public ProjectExplorerVM(MainWindowViewModel mainWindowViewModel, ResultsViewAreaVM ResultsViewAreaVM, Project project) : base("Обозреватель проекта")
        {
            Project = project;
            _mainWindowViewModel = mainWindowViewModel;
            ProjectExplorerTreeViewVM = new ProjectExplorerTreeViewVM(Project, ResultsViewAreaVM);
        }

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model => Model.GetModel();
        MainWindowViewModel _mainWindowViewModel;
        Project Project;
        #endregion


        #region Properties

        ProjectExplorerTreeViewVM projectExplorerTreeViewVM;
        public ProjectExplorerTreeViewVM ProjectExplorerTreeViewVM 
        {
            get
            {
                return projectExplorerTreeViewVM;
            }
            set 
            {
                OnPropertyChanged("ProjectExplorerTreeViewVM");
                projectExplorerTreeViewVM = value;
            }
        }
        #endregion
        #region Methods
        #endregion

        #region Commands

        private RelayCommand addDiagramViewCommand;
        public RelayCommand AddDiagramViewCommand
        {
            get
            {
                return addDiagramViewCommand ?? (addDiagramViewCommand = new RelayCommand(obj =>
                {
                    _mainWindowViewModel.AddResultsViewCommand.Execute(obj);
                }));
            }
        }
        #endregion
    }
}
