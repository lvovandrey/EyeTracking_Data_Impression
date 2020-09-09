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
        /// Средняя продолжительность нахождения взгляда в каждом FAOI. "Среднее время фиксации в зоне"
        /// </summary>
        public FAOIDistributed_Parameter<TimeSpan> AverageFixationTimeDistribution
        {
            get { return AverageFixationTimeDistributionCalculate(); }
        }

        /// <summary>
        /// Продолжительность файла. "Полное время анализируемой записи"
        /// </summary>
        public ScalarParameter<TimeSpan> FullTime
        {
            get { return new ScalarParameter<TimeSpan>("Полное время анализируемого интервала", FullTimeCalculate()); }
        }

        /// <summary>
        /// Полное количество переходов взгляда между функциональными зонами
        /// </summary>
        public ScalarParameter<int> FixationsFullCount
        {
            get { return new ScalarParameter<int>("Полное количество переходов взгляда между функциональными зонами", FixationsFullCountCalculate()); }
        }
        /// <summary>
        /// Частота переходов взгляда между функциональными зонами,1/мин
        /// </summary>
        public ScalarParameter<double> ChangeFAOIFrequencyPerMinute
        {
            get { return new ScalarParameter<double>("Частота переходов взгляда между функциональными зонами, 1/мин", ChangeFAOIFrequencyPerMinuteCalculate()); }
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

        #region Methods

        /// <summary>
        /// Функция расчета параметра "Доля времени в функциональной зоне"
        /// </summary>
        /// <returns></returns>
        private FAOIDistributed_Parameter<double> TimePercentDistributionCalculate()
        {
            FAOIDistributed_Parameter<double> timePercentDistribution = new FAOIDistributed_Parameter<double>("Доля времени в функциональной зоне, %");
            if (FAOIHitsOnTimeIntervalList == null) return timePercentDistribution;
            if (FAOIHitsOnTimeIntervalList.Count() < 1) throw new Exception("Неполные данные: ProcessingResults.FAOIHitsOnTimeIntervalList не содержит ни одного элемента");
            TimeSpan tbeg = FAOIHitsOnTimeIntervalList[0].TimeInterval.TimeBegin;
            TimeSpan tend = FAOIHitsOnTimeIntervalList[FAOIHitsOnTimeIntervalList.Count() - 1].TimeInterval.TimeEnd;
            if (tend <= tbeg) throw new Exception("Неверные данные: неверно задано полное время анализируемого файла или заданный интервал времени: начало " +
                  tbeg.TotalMilliseconds.ToString() + "ms конец " + tend.TotalMilliseconds.ToString());
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
                result.Value = SummCurFAOITime.TotalMilliseconds / SummTime.TotalMilliseconds;
                timePercentDistribution.Results.Add(result);
            }
            return timePercentDistribution;
        }

        /// <summary>
        /// Функция расчета параметра "Среднее время фиксации в зоне"
        /// </summary>
        /// <returns></returns>
        private FAOIDistributed_Parameter<TimeSpan> AverageFixationTimeDistributionCalculate()
        {
            FAOIDistributed_Parameter<TimeSpan> averageFixationTimeDistribution = new FAOIDistributed_Parameter<TimeSpan>("Среднее время фиксации в зоне");
            if (FAOIHitsOnTimeIntervalList == null) return averageFixationTimeDistribution;
            if (FAOIHitsOnTimeIntervalList.Count() < 1) throw new Exception("Неполные данные: ProcessingResults.FAOIHitsOnTimeIntervalList не содержит ни одного элемента");
            TimeSpan tbeg = FAOIHitsOnTimeIntervalList[0].TimeInterval.TimeBegin;
            TimeSpan tend = FAOIHitsOnTimeIntervalList[FAOIHitsOnTimeIntervalList.Count() - 1].TimeInterval.TimeEnd;
            if (tend <= tbeg) throw new Exception("Неверные данные: неверно задано полное время анализируемого файла или заданный интервал времени: начало " +
                  tbeg.TotalMilliseconds.ToString() + "ms конец " + tend.TotalMilliseconds.ToString());
            TimeSpan SummTime = tend - tbeg;

            foreach (var curFAOI in SourceData.FAOIs)
            {
                TimeSpan SummCurFAOITime = TimeSpan.Zero;
                int FixationsCount = 0;
                foreach (var fAOIHitsOnTimeInterval in FAOIHitsOnTimeIntervalList)
                {
                    if (fAOIHitsOnTimeInterval.FAOIHits.Contains(curFAOI))
                    {
                        SummCurFAOITime += fAOIHitsOnTimeInterval.TimeInterval.Duration();
                        FixationsCount++;
                    }
                }
                var result = new FAOI_Value_Pair<TimeSpan>();
                result.FAOI = curFAOI;
                result.Value = TimeSpan.FromMilliseconds(SummCurFAOITime.TotalMilliseconds / FixationsCount);
                averageFixationTimeDistribution.Results.Add(result);
            }
            return averageFixationTimeDistribution;
        }

        /// <summary>
        /// Вычисляет полную продолжительность обрабатываемого файла
        /// </summary>
        /// <returns></returns>
        private TimeSpan FullTimeCalculate()
        {
            if (FAOIHitsOnTimeIntervalList == null) throw new Exception("Неполные данные: ProcessingResults.FAOIHitsOnTimeIntervalList не создан");
            if (FAOIHitsOnTimeIntervalList.Count() < 1) throw new Exception("Неполные данные: ProcessingResults.FAOIHitsOnTimeIntervalList не содержит ни одного элемента");
            TimeSpan tbeg = FAOIHitsOnTimeIntervalList[0].TimeInterval.TimeBegin;
            TimeSpan tend = FAOIHitsOnTimeIntervalList[FAOIHitsOnTimeIntervalList.Count() - 1].TimeInterval.TimeEnd;
            if (tend <= tbeg) throw new Exception("Неверные данные: неверно задано полное время анализируемого файла или заданный интервал времени: начало " +
                  tbeg.TotalMilliseconds.ToString() + "ms конец " + tend.TotalMilliseconds.ToString());
            return tend - tbeg;
        }



        /// <summary>
        /// Функция для расчета параметра "Полное количество переходов взгляда между функциональными зонами"
        /// </summary>
        /// <returns></returns>
        private int FixationsFullCountCalculate()
        {
            if (FAOIHitsOnTimeIntervalList == null) throw new Exception("Неполные данные: ProcessingResults.FAOIHitsOnTimeIntervalList равен NULL");
            if (FAOIHitsOnTimeIntervalList.Count() < 1) throw new Exception("Неполные данные: ProcessingResults.FAOIHitsOnTimeIntervalList не содержит ни одного элемента");
            int fixationsCount=0;
            for (int i = 1; i < FAOIHitsOnTimeIntervalList.Count-1; i++)
            {
                var f = FAOIHitsOnTimeIntervalList[i];
                var fprev = FAOIHitsOnTimeIntervalList[i-1];

                FAOI faoi = new FAOI();
                FAOI faoiprev = new FAOI();

                if (f.FAOIHits.Count > 0) faoi = f.FAOIHits[0];
                if (fprev.FAOIHits.Count > 0) faoiprev = fprev.FAOIHits[0];

                if (faoi.Equals(faoiprev)) fixationsCount++;

            }
            return fixationsCount;
        }

        /// <summary>
        /// Функция для расчета параметра "Частота переходов взгляда между функциональными зонами, 1/мин"
        /// </summary>
        /// <returns></returns>
        private double ChangeFAOIFrequencyPerMinuteCalculate()
        {
            return FixationsFullCount.Value / FullTime.Value.TotalMinutes;
        }
        #endregion


    }
}
