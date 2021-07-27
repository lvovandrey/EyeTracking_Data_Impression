using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelExporter
{
    interface IExportable
    {
        ProcessingResults Results { get; }
        string FileName { get; set; }
        void GenerateExcelReport();
    }
}
