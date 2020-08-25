﻿using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.ViewModel.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataImpression.ViewModel
{
    public class ResultsViewAreaVM: INPCBase
    {
        #region ctor
        public ResultsViewAreaVM(Model model, MainWindowViewModel mainWindowViewModel)
        {
            _model = model;
            _this = this;
            DiagramVM = new FAOIDiagramVM(model);
            _mainWindowViewModel = mainWindowViewModel;
        }

        FAOIDiagramVM diagramVM;
        public FAOIDiagramVM DiagramVM { get { return diagramVM; } set { diagramVM = value; OnPropertyChanged("DiagramVM"); } }

        private MainWindowViewModel _mainWindowViewModel;


        static ResultsViewAreaVM _this;

        public static ResultsViewAreaVM This
        {
            get { return _this; }
        }


        #endregion

        #region Fields
        /// <summary>
        /// Модель данных
        /// </summary>
        private Model _model;

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

        #region НадоПоменятьПотом

        ObservableCollection<DocumentViewVM> documentViewVMs = new ObservableCollection<DocumentViewVM>();
        ReadOnlyObservableCollection<DocumentViewVM> readonyDocumentViewVMs = null;
        public ReadOnlyObservableCollection<DocumentViewVM> DocumentViewVMs
        {
            get
            {
                if (readonyDocumentViewVMs == null)
                    readonyDocumentViewVMs = new ReadOnlyObservableCollection<DocumentViewVM>(documentViewVMs);

                return readonyDocumentViewVMs;
            }
        }




        ToolVM[] _tools = null;

        public IEnumerable<ToolVM> Tools
        {
            get
            {
                if (_tools == null)
                    _tools = new ToolVM[] { ProjectExplorer };
                return _tools;
            }
        }

      
        ProjectExplorerVM _projectExplorer = null;
        public ProjectExplorerVM ProjectExplorer
        {
            get
            {
                if (_projectExplorer == null)
                    _projectExplorer = new ProjectExplorerVM(_mainWindowViewModel, _model);

                return _projectExplorer;
            }
        }


        internal void ResultViewsAdd(string ParameterName)
        {
            documentViewVMs.Add(new DocumentViewVM(_model, ParameterName));
            ActiveDocument = documentViewVMs.Last();
        }



        private DocumentViewVM _activeDocument = null;
        public DocumentViewVM ActiveDocument
        {
            get { return _activeDocument; }
            set
            {
                if (_activeDocument != value)
                {
                    _activeDocument = value;
                    OnPropertyChanged("ActiveDocument");
                    if (ActiveDocumentChanged != null)
                        ActiveDocumentChanged(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler ActiveDocumentChanged;



        #endregion

        internal bool CanExecuteNextInputStage()
        {
            return true;
        }
        #endregion
    }
}