using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.View.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public class ProjectExplorerVM: ToolVM
    {
        public ProjectExplorerVM(MainWindowViewModel mainWindowViewModel, Model model) : base("Project Explorer")
        {
            _model = model;
            _mainWindowViewModel = mainWindowViewModel;
        }

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        Model _model;
        MainWindowViewModel _mainWindowViewModel;
        #endregion


        #region Properties

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
