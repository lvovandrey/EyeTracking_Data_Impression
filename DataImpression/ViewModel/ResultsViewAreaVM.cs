using DataImpression.AbstractMVVM;
using DataImpression.Models;
using DataImpression.View.AvalonDockHelpers;
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

        /// это надо будет переделать
        ObservableCollection<FileVM> _files = new ObservableCollection<FileVM>();
        ReadOnlyObservableCollection<FileVM> _readonyFiles = null;
        public ReadOnlyObservableCollection<FileVM> Files
        {
            get
            {
                if (_readonyFiles == null)
                    _readonyFiles = new ReadOnlyObservableCollection<FileVM>(_files);

                return _readonyFiles;
            }
        }

        ObservableCollection<TestDiagramVM> testDiargamVMs = new ObservableCollection<TestDiagramVM>();
        ReadOnlyObservableCollection<TestDiagramVM> readonyTestDiargamVMs = null;
        public ReadOnlyObservableCollection<TestDiagramVM> TestDiargamVMs
        {
            get
            {
                if (readonyTestDiargamVMs == null)
                    readonyTestDiargamVMs = new ReadOnlyObservableCollection<TestDiagramVM>(testDiargamVMs);

                return readonyTestDiargamVMs;
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

        FileStatsVM _fileStats = null;
        public FileStatsVM FileStats
        {
            get
            {
                if (_fileStats == null)
                    _fileStats = new FileStatsVM();

                return _fileStats;
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


        internal void ResultViewsAdd(string v)
        {
            testDiargamVMs.Add(new TestDiagramVM(_model));
            ActiveDocument = testDiargamVMs.Last();

            _files.Add(new FileVM());


        }



        private TestDiagramVM _activeDocument = null;
        public TestDiagramVM ActiveDocument
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

        public ObservableCollection<FileVM> ResultViews = new ObservableCollection<FileVM>();



        internal bool CanExecuteNextInputStage()
        {
            return true;
        }
        #endregion
    }
}
