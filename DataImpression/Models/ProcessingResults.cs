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
        public ProcessingResults(ProcessingTaskSourceData _sourceData)
        {
            SourceData = _sourceData;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Источник данных
        /// </summary>
        public ProcessingTaskSourceData SourceData { get; set; }

        /// <summary>
        /// Распределение суммарного относительног времени фиксаций (в виде доли от полного времени) по функциональным зонам
        /// т.е. если value = 0.5 для данной faoi, значит туда смотрели половину от всего времени
        /// </summary>
        public FAOIDistributed_Parameter<double> TimePercentDistribution
        {
            get { return TimePercentDistributionCalculate(); }
        }

        /// <summary>
        /// Список с данными после предварительной обработки - содержит информацию в какие FAOI он смотрел в какие интервалы времени
        /// </summary>
        public List<FAOIHitsOnTimeInterval> FAOIHitsOnTimeIntervalList { get; set; } 

        /// <summary>
        /// Список с данными после предварительной обработки - содержит отфильтрованную (без повторов в промежутках) 
        /// информацию в каких колонках AOIhits имелись отметки о фиксации в какие моменты времени
        /// </summary>
        public List<TobiiCSVRecord> TobiiCSVRecordsList { get; set; }

        #endregion

        #region Methods
        #endregion

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Функция расчета параметра "Доля времени в функциональной зоне"
        /// </summary>
        /// <returns></returns>
        private FAOIDistributed_Parameter<double> TimePercentDistributionCalculate()
        {
            FAOIDistributed_Parameter<double> timePercentDistribution = new FAOIDistributed_Parameter<double>("Доля времени в функциональной зоне, %");

            if (FAOIHitsOnTimeIntervalList.Count() < 1) throw new Exception("Неполные данные: ProcessingResults.FAOIHitsOnTimeIntervalList не содержит ни одного элемента");
            TimeSpan tbeg = FAOIHitsOnTimeIntervalList[0].TimeInterval.TimeBegin;
            TimeSpan tend = FAOIHitsOnTimeIntervalList[FAOIHitsOnTimeIntervalList.Count()-1].TimeInterval.TimeEnd;
            if (tend<=tbeg) throw new Exception("Неверные данные: неверно задано полное время анализируемого файла или заданный интервал времени: начало " +
                tbeg.TotalMilliseconds.ToString() +  "ms конец " + tend.TotalMilliseconds.ToString());
            TimeSpan SummTime = tend - tbeg;

            foreach (var curFAOI in SourceData.FAOIs)
            {
                TimeSpan SummCurFAOITime = TimeSpan.Zero;
                foreach (var fAOIHitsOnTimeInterval in FAOIHitsOnTimeIntervalList)
                {
                    if (fAOIHitsOnTimeInterval.FAOIHits.Contains(curFAOI))
                        SummCurFAOITime += fAOIHitsOnTimeInterval.TimeInterval.Duration();
                }
                var result = new FAOI_Value_Pair<double>();
                result.FAOI = curFAOI;
                result.Value = SummCurFAOITime.TotalMilliseconds/ SummTime.TotalMilliseconds;
                timePercentDistribution.Results.Add(result);
            }
            return timePercentDistribution;
        }

        #endregion


    }
}
