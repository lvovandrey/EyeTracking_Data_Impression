using DataImpression.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelExporter
{
    public class Exporter : IExportable
    {
        public Exporter(string filename)
        {
            fileName = filename;
        }
        public ProcessingResults Results
        {
            get
            {
                if (Model.GetModel().HaveData)
                    return Model.GetModel().Results;
                else return null;
            }
        }

        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }//TODO: Валидация
        }

        public void GenerateExcelReport()
        {
            File.WriteAllBytes(FileName, Generate());
        }


        private byte[] Generate()
        {
            var package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets
                .Add("Market Report");

            sheet.Cells["B2"].Value = Results.SourceData.CSVFileName;

            return package.GetAsByteArray();
        }
    }

}
