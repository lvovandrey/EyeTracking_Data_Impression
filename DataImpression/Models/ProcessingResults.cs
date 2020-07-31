using DataImpression.Models.ResultTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Инкапсулирует все результаты решения одной отдельной
    /// задачи по обработке.
    /// </summary>
    public class ProcessingResults
    {
        #region ctor
        public ProcessingResults()
        {
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Распределение суммарного относительног времени фиксаций (в виде доли от полного времени) по функциональным зонам
        /// т.е. если value = 0.5 для данной faoi, значит туда смотрели половину от всего времени
        /// </summary>
        public FAOIDistributed_Parameter<double> TimePercentDistribution { get; set; }

        /// <summary>
        /// Список с данными после обработки - содержит информацию в какие FAOI он смотрел в какие моменты времени
        /// </summary>
        public List<FAOIHitsOnTimeInterval> FAOIHitsOnTimeIntervalList { get; set; } //можно было бы сделать Dictionary<TimeInterval, List<FAOI>> но мне кажется так нагляднее и проще воспринимать

        #endregion

        #region Methods
        #endregion

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods


        #endregion


    }
}
