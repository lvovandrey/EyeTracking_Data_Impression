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
        public ResultsViewAreaVM()
        {
            _this = this;
        }

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

        ToolVM[] _tools = null;

        public IEnumerable<ToolVM> Tools
        {
            get
            {
                if (_tools == null)
                    _tools = new ToolVM[] { FileStats };
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


        internal void ResultViewsAdd(string v)
        {
            _files.Add(new FileVM());
            ActiveDocument = _files.Last();

            ResultViews.Add(new FileVM());
            OnPropertyChanged("ResultViews");
            Test++; OnPropertyChanged("Test");

        }



        private FileVM _activeDocument = null;
        public FileVM ActiveDocument
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

        internal void ResultViewsAdd_bad(string v)
        {
            ResultViews.Add(new FileVM());
            OnPropertyChanged("ResultViews");
            Test++; OnPropertyChanged("Test");

        }

        public int Test { get; private set; } = 3;
        #endregion
    }
}
