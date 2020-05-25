using DataImpression.AbstractMVVM;
using DataImpression.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;

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
        /// <summary>
        /// Метод для команды OpenCSVFileCommand - открывает csv файл и готовится к работе с ним (заполняет CSVCaption и CSVFileName в SourceData в модели). 
        /// </summary>
        public void OpenCSVFile()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog()==false) return;
            _model.SourceData.CSVFileName = openFileDialog.FileName;// файлнейм в модель  закидываем
            try
            { 
                List<string> caption_string = new CSVReader().TobiiCSVReadStrings(_model.SourceData.CSVFileName, 1);//читаем первую строку и далее ее разбиваем.
                List<string> splitted_caption_string = new List<string>(caption_string[0].Split('\t'));//разбиваем ее

                _model.SourceData.CSVCaption = Column.ToColumns(splitted_caption_string);//и преобразовываем в набор колонок и закидываем в модель
                
            }
            catch
            {
                MessageBox.Show("Не удалось считать заголовок csv-файла. Попробуйте открыть файл вручную и убедиться в правильности его формата.");
            }
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
