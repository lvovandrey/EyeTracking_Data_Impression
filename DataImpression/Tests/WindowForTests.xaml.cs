using DataImpression.Models;
using DataImpression.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace DataImpression.Tests
{
    /// <summary>
    /// Логика взаимодействия для WindowForTests.xaml
    /// </summary>
    public partial class WindowForTests : Window
    {
        public WindowForTests()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveToXML(  ((MainWindowViewModel)DataContext).GetModel().Results);

        }

 


        private void SaveToXML(ProcessingResults Results)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Etprj-flie(*.Etprj)|*.Etprj";
            bool? res = saveFileDialog.ShowDialog();
            if (res == null || res == false) return;
            string filename = saveFileDialog.FileName;

            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(ProcessingResults));

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    formatter.Serialize(fs, Results);
                }
                MessageBox.Show("Файл " + filename + " успешно сохранен");
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка сохранения файла " + filename + ". \n Описание ошибки: " + e.Message + "    Stacktrace:" + e.StackTrace);
            }
        }

        private void LoadFromXML()
        {
            ProcessingResults results;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Etprj-flie(*.Etprj)|*.Etprj";
            bool? res = openFileDialog.ShowDialog();
            if (res == null || res == false) return;
            string filename = openFileDialog.FileName;
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(ProcessingResults));

                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    results = (ProcessingResults)formatter.Deserialize(fs);
                }

                Model.GetModel().Results = results;
                Model.GetModel().SourceData = results.SourceData;
                Project project = new Project(filename);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка открытия файла " + filename + ". \n Описание ошибки: " + e.Message + "    Stacktrace:" + e.StackTrace);
            }
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadFromXML();
        }
    }
}
