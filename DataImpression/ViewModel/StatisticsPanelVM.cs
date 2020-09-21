using DataImpression.Models;
using DataImpression.Models.ResultTypes;
using DataImpression.ViewModel.AvalonDockHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.ViewModel
{
    public class StatisticsPanelVM : ToolVM
    {
        public StatisticsPanelVM(Model _model) : base("Панель статистики")
        {
            model = _model;
        }

        public Model model { get; private set; }


        public object FullTime
        {
            get
            {
                return new
                {
                    Val = model.Results.FullTime.Value,
                    Name = model.Results.FullTime.ParameterName
                };
            }
        }

        public object FixationsFullCount
        {
            get
            {
                return new
                {
                    Val = model.Results.FixationsFullCount.Value,
                    Name = model.Results.FixationsFullCount.ParameterName
                };
            }
        }

        public object FrequencyRequestsToAnyFAOIPerMinute
        {
            get
            {
                return new
                {
                    Val = model.Results.FrequencyRequestsToAnyFAOIPerMinute.Value,
                    Name = model.Results.FrequencyRequestsToAnyFAOIPerMinute.ParameterName
                };
            }
        }

        public ObservableCollection<string> OptionalParams
        {
            get
            {
                var coll = new ObservableCollection<string>();
                foreach (var item in model.Results.OptionalParameters)
                    coll.Add(item.Key + " - " + item.Value.ToString());

                return coll;
            }
        }

    }

}
