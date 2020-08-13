using DataImpression.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace DataImpression.View.AvalonDockHelpers
{
    public class FileStatsVM : ToolVM
    {
        public FileStatsVM()
            : base("File Stats")
        {
            ResultsViewAreaVM.This.ActiveDocumentChanged += new EventHandler(OnActiveDocumentChanged);
            ContentId = ToolContentId;

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.UriSource = new Uri("pack://application:,,/Images/Table-Chart.png");
            bi.EndInit();
            IconSource = bi;
        }

        public const string ToolContentId = "FileStatsTool";

       

        #region FileSize

        private long _fileSize;
        public long FileSize
        {
            get { return _fileSize; }
            set
            {
                if (_fileSize != value)
                {
                    _fileSize = value;
                    OnPropertyChanged("FileSize");
                }
            }
        }

        #endregion

        #region LastModified

        private DateTime _lastModified;
        public DateTime LastModified
        {
            get { return _lastModified; }
            set
            {
                if (_lastModified != value)
                {
                    _lastModified = value;
                    OnPropertyChanged("LastModified");
                }
            }
        }

        #endregion

        void OnActiveDocumentChanged(object sender, EventArgs e)
        {

                FileSize = new Random().Next();
                LastModified = DateTime.MinValue;
        }


    }
}
