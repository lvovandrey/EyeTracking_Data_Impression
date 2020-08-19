using DataImpression.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public class FAOIDistributedComplexColumnChartVM<T>: DocumentBodyVM
    {
        public FAOIDistributedComplexColumnChartVM(Model model):base(model)
        {
            ChartVMs = new ObservableCollection<dynamic>();
        }
        public ObservableCollection<dynamic> ChartVMs { get; set; }
    }
}
