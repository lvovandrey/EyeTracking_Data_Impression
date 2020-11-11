using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace DataImpression.Models.Helpers
{
    public static class ModelSerializer
    {

        public static void SaveToXML(Project project)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Etprj-flie(*.Etprj)|*.Etprj";
            bool? res = saveFileDialog.ShowDialog();
            if (res == null || res == false) return;
            string filename = saveFileDialog.FileName;

            SaveToXML(project, filename);
        }

        public static void SaveToXML(Project project, string filename)
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Project));

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    formatter.Serialize(fs, project);
                }
                MessageBox.Show("Файл " + filename + " успешно сохранен");
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка сохранения файла " + filename + ". \n Описание ошибки: " + e.Message + "    Stacktrace:" + e.StackTrace);
            }
        }

        public static void LoadFromXML(ref Project _project)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Etprj-flie(*.Etprj)|*.Etprj";
            bool? res = openFileDialog.ShowDialog();
            if (res == null || res == false) return;

            ProcessingResults results;
            Model.ClearModel();
            Project project = new Project();
            _project = project;


            string filename = openFileDialog.FileName;
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Project));

                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    results = ((Project)formatter.Deserialize(fs)).Model.Results;
                }

                Model.GetModel().Results = results;
                Model.GetModel().SourceData = results.SourceData;
                project = new Project(filename);

            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка открытия файла " + filename + ". \n Описание ошибки: " + e.Message + "    Stacktrace:" + e.StackTrace);
            }
            _project = project;
        }
    }
}
