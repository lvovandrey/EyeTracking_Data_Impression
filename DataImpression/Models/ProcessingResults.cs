using DataImpression.Models.Helpers;
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
    [Serializable]
    public class ProcessingResults
    {
        #region ctor
        public ProcessingResults(ProcessingTaskSourceData _sourceData)
        {
            SourceData = _sourceData;
        }
        public ProcessingResults()
        {

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
            get 
            {
                var timePercentDistribution = new FAOIDistributed_Parameter<double>("Нет данных");
                try
                {
                    timePercentDistribution = TimePercentDistributionCalculate();
                }
                catch (Exception e)
                {
                    Logger.Write(e.Message);
                }
                return timePercentDistribution; 
            }
        }

        /// <summary>
        /// Средняя продолжительность нахождения взгляда в каждом FAOI. "Среднее время фиксации в зоне"
        /// </summary>
        public FAOIDistributed_Parameter<TimeSpan> AverageFixationTimeDistribution
        {
            get
            {
                var averageFixationTimeDistribution = new FAOIDistributed_Parameter<TimeSpan>("Нет данных");
                try
                {
                    averageFixationTimeDistribution = AverageFixationTimeDistributionCalculate();
                }
                catch (Exception e)
                {
                    Logger.Write(e.Message);
                }
                return averageFixationTimeDistribution; 
            }
        }

        /// <summary>
        /// Распределение частоты обращения к каждой функциональной зоне. "Частота обращения к зоне, 1/мин"
        /// </summary>
        public FAOIDistributed_Parameter<double> FrequencyRequestsFAOIDistributionPerMinute
        {
            get 
            {
                var frequencyRequestsFAOIDistributionPerMinute = new FAOIDistributed_Parameter<double>("Нет данных");
                try
                {
                    frequencyRequestsFAOIDistributionPerMinute = FrequencyRequestsFAOIDistributionPerMinuteCalculate();
                }
                catch (Exception e)
                {
                    Logger.Write(e.Message);
                }
                return frequencyRequestsFAOIDistributionPerMinute; 
            }
        }


        /// <summary>
        /// Продолжительность файла. "Полное время анализируемой записи"
        /// </summary>
        public ScalarParameter<TimeSpan> FullTime
        {
            get 
            {
                var fulltime = TimeSpan.Zero;
                try
                {
                    fulltime = FullTimeCalculate();
                }
                catch(Exception e)
                {
                    Logger.Write(e.Message);
                }
                return new ScalarParameter<TimeSpan>("Полное время анализируемого интервала", fulltime);
            }
        }

        /// <summary>
        /// Полное количество переходов взгляда между функциональными зонами
        /// </summary>
        public ScalarParameter<int> FixationsFullCount
        {
            get 
            {
                var fixationsFullCount = 0;
                try
                {
                    fixationsFullCount = FixationsFullCountCalculate();
                }
                catch (Exception e)
                {
                    Logger.Write(e.Message);
                }
                return new ScalarParameter<int>("Полное количество переходов взгляда между функциональными зонами", fixationsFullCount); 
            }
        }
        /// <summary>
        /// Частота переходов взгляда между функциональными зонами,1/мин
        /// </summary>
        public ScalarParameter<double> FrequencyRequestsToAnyFAOIPerMinute
        {
            get 
            {
                var frequencyRequestsToAnyFAOIPerMinute = 0.0;
                try
                {
                    frequencyRequestsToAnyFAOIPerMinute = FrequencyRequestsToAnyFAOIPerMinuteCalculate();
                }
                catch (Exception e)
                {
                    Logger.Write(e.Message);
                }
                return new ScalarParameter<double>("Частота переходов взгляда между функциональными зонами, 1/мин", frequencyRequestsToAnyFAOIPerMinute); }
        }

        /// <summary>
        /// Дополнительные параметры - типа имени испытуемого и прочей ерунды
        /// </summary>
        public XmlSerializableDictionary<string, object> OptionalParameters
        {
            get 
            {
                XmlSerializableDictionary<string, object> optionalParameters = new XmlSerializableDictionary<string, object>();
                try
                {
                    optionalParameters = OptionalParametersCalculate();
                }
                catch (Exception e)
                {
                    Logger.Write(e.Message);
                }
                return optionalParameters; 
            }
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
        /// Функция расчета параметра "Частота обращения к зоне, 1/мин"
        /// </summary>
        private FAOIDistributed_Parameter<double> FrequencyRequestsFAOIDistributionPerMinuteCalculate()
        {
            FAOIDistributed_Parameter<double> frequencyDistribution = new FAOIDistributed_Parameter<double>("Частота обращения к зоне, 1/мин");
            double fullTimeMinutes = FullTime.Value.TotalMinutes; //обратимся сразу к этому полю, чтобы если что прошла валидация

            foreach (var faoi in SourceData.FAOIs)
            {
                int fixationsCount = 0;
                for (int i = 0; i < FAOIHitsOnTimeIntervalList.Count; i++)
                {

                    if ((FAOIHitsOnTimeIntervalList[i].FAOIHits.Contains(faoi)))
                    {
                        fixationsCount++;
                        do
                        {
                            i++;
                            if (FAOIHitsOnTimeIntervalList.Count <= i)
                                break;
                        } while (FAOIHitsOnTimeIntervalList[i].FAOIHits.Contains(faoi));
                    }
                }
                frequencyDistribution.Results.Add(new FAOI_Value_Pair<double>() { FAOI = faoi, Value = fixationsCount / fullTimeMinutes });
            }

            return frequencyDistribution;
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
            int fixationsCount = 0;
            for (int i = 1; i < FAOIHitsOnTimeIntervalList.Count; i++)
            {
                var f = FAOIHitsOnTimeIntervalList[i];
                var fprev = FAOIHitsOnTimeIntervalList[i - 1];

                FAOI faoi = new FAOI();
                FAOI faoiprev = new FAOI();

                if (f.FAOIHits.Count > 0) faoi = f.FAOIHits[0];
                if (fprev.FAOIHits.Count > 0) faoiprev = fprev.FAOIHits[0];

                if (!faoi.Equals(faoiprev)) fixationsCount++;

            }
            return fixationsCount;
        }

        /// <summary>
        /// Функция для расчета параметра "Частота переходов взгляда между функциональными зонами, 1/мин"
        /// </summary>
        /// <returns></returns>
        private double FrequencyRequestsToAnyFAOIPerMinuteCalculate()
        {
            return FixationsFullCount.Value / FullTime.Value.TotalMinutes;
        }


        /// <summary>
        /// Функция для чтения дополнительных параметров из csv-файла.
        /// </summary>
        /// <returns></returns>
        private XmlSerializableDictionary<string, object> OptionalParametersCalculate()
        {
            if(SourceData.CSVSecondString==null || SourceData.CSVSecondString =="") 
                throw new Exception("Неполные данные: SourceData.CSVSecondString");
            if (SourceData.OptionalDataCSVColumns==null || SourceData.OptionalDataCSVColumns.Count==0)
                throw new Exception("Неполные данные: SourceData.OptionalDataCSVColumns");
            char delimiter = '\t';
            string[] strs = SourceData.CSVSecondString.Split(delimiter);
            var Params = new XmlSerializableDictionary<string, object>();
            foreach (var item in SourceData.OptionalDataCSVColumns)
            {
                if (item.Value.OrderedNumber < 0) continue;
                if (strs.Length - 1 < item.Value.OrderedNumber) continue;
                Params.Add(item.Key, strs[item.Value.OrderedNumber]);
            }

            return Params;
        }

        #endregion




    }
}
