using DataImpression.Models;
using DataImpression.Models.ResultTypes;
using DataImpression.ViewModel.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public class StatisticsPanelVM: ToolVM
    {
        public StatisticsPanelVM(Model _model):base("Панель статистики")
        {
            model = _model;
            OnPropertyChanged("FullTime");
            OnPropertyChanged("FixationsFullCount");
            OnPropertyChanged("FrequencyRequestsToAnyFAOIPerMinute");
            OnPropertyChanged("FullTimeName");
            OnPropertyChanged("FixationsFullCountName");
            OnPropertyChanged("FrequencyRequestsToAnyFAOIPerMinuteName");
        }
        
        public Model model { get; private set; }
        


        public TimeSpan FullTime
        {
            get { return model.Results.FullTime.Value; }
        }


        public int FixationsFullCount
        {
            get { return model.Results.FixationsFullCount.Value; }
        }


        public double FrequencyRequestsToAnyFAOIPerMinute
        {
            get { return model.Results.FrequencyRequestsToAnyFAOIPerMinute.Value; }
        }

        public string FullTimeName
        {
            get { return model.Results.FullTime.ParameterName; }
        }

        public string FixationsFullCountName
        {
            get { return model.Results.FixationsFullCount.ParameterName; }
        }

        public string FrequencyRequestsToAnyFAOIPerMinuteName
        {
            get { return model.Results.FrequencyRequestsToAnyFAOIPerMinute.ParameterName; }
        }



    }
}
