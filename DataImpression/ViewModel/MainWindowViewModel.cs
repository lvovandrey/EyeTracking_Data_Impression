using DataImpression.AbstractMVVM;
using DataImpression.Models;
using Microsoft.Win32;

namespace DataImpression.ViewModel
{
    public class MainWindowViewModel:INPCBase
    {

        #region ctor
        public MainWindowViewModel(Model model)
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
        #endregion

        #region Methods
        public void OpenCSVFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog()==false) return;
            _model.SourceData.CSVFileName = openFileDialog.FileName;

        }
        #endregion

        #region Commands


        private RelayCommand openCSVFileCommand;
        public RelayCommand OpenCSVFileCommand
        {
            get
            {
                return openCSVFileCommand ?? (openCSVFileCommand = new RelayCommand(obj =>
                {
                    OpenCSVFile();
                }));
            }
        }


        #endregion
    }
}
