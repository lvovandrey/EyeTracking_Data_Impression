using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.UseCases
{
    public class ResultsReporter
    {
        public ProcessingResults GetReport()
        {
            if (Model.GetModel().HaveData)
                return Model.GetModel().Results;
            else return null;
        }
    }
}
