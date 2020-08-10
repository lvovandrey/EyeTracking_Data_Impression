using DataImpression.AbstractMVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataImpression.View.AvalonDockHelpers
{
    class FileVM : PaneVM
    {
        static ImageSourceConverter ISC = new ImageSourceConverter();
        public FileVM(string filePath)
        {
            FilePath = filePath;
            Title = FileName;

            //Set the icon only for open documents (just a test)
            IconSource = ISC.ConvertFromInvariantString(@"pack://application:,,/Images/Table-Add.png") as ImageSource;
        }

        public FileVM()
        {
            IsDirty = true;
            Title = FileName;
        }

        #region FilePath
        private string _filePath = null;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath != value)
                {
                    _filePath = value;
                    OnPropertyChanged("FilePath");
                    OnPropertyChanged("FileName");
                    OnPropertyChanged("Title");

                    if (File.Exists(_filePath))
                    {
                        _textContent = File.ReadAllText(_filePath);
                        ContentId = _filePath;
                    }
                }
            }
        }
        #endregion


        public string FileName
        {
            get
            {
                if (FilePath == null)
                    return "Noname" + (IsDirty ? "*" : "");

                return System.IO.Path.GetFileName(FilePath) + (IsDirty ? "*" : "");
            }
        }



        #region TextContent

        private string _textContent = string.Empty;
        public string TextContent
        {
            get { return _textContent; }
            set
            {
                if (_textContent != value)
                {
                    _textContent = value;
                    OnPropertyChanged("TextContent");
                    IsDirty = true;
                }
            }
        }

        #endregion

        #region IsDirty

        private bool _isDirty = false;
        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    OnPropertyChanged("IsDirty");
                    OnPropertyChanged("FileName");
                }
            }
        }

        #endregion



    }
}
