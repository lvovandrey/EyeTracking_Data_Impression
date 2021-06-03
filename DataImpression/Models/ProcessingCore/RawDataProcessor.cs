using DataImpression.Models.ProcessingCore;
using DataImpression.Models.ResultTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DataImpression.Models
{
    /// <summary>
    /// Это класс для обработки сырых данных
    /// Мне показалось, будет лучше не засорять SourceData а вынести его наружу.
    /// </summary>
    public class RawDataProcessor
    {

        #region ctor
        public RawDataProcessor(ProcessingTaskSourceData sourceData, ProcessingResults results)
        {
            SourceData = sourceData;
            Results = results;
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
        /// Результат
        /// </summary>
        public ProcessingResults Results { get; set; }

        #endregion

        #region Methods
        public void ConvertCSVRawDataToFAOIHitsOnTimeIntervalList(ref double progress, ref string stage)
        {
            if (SourceData.CSVAOIHitsColumns == null) throw new Exception("Incomplete data: SourceData.CSVAOIHitsColumns");
            if (SourceData.WorkingTOI == null) throw new Exception("Incomplete data: SourceData.WorkingTOI");
            if (SourceData.FAOIs == null) throw new Exception("Incomplete data: SourceData.FAOIs");
            if (SourceData.CSVColumnsToFAOIsConversionTable == null) throw new Exception("Incomplete data: SourceData.CSVColumnsToFAOIsConversionTable");
            if (SourceData.CSVTimeColumn == null) throw new Exception("Incomplete data: SourceData.CSVTimeColumn");


            RawDataProcessorMethods.TobiiCSVReadSecondString(SourceData);
            progress = 0; stage = "Оценка размера файла";
            long countStringsForReading = RawDataProcessorMethods.TobiiCSVCalculateCountOfStrings(SourceData);
            progress = 5; stage = "Считывание файла " + SourceData.CSVFileName;
            List<TobiiCSVRecord> tobiiCSVRecords = RawDataProcessorMethods.TobiiCSVRead(SourceData, ref progress, 20, countStringsForReading + 100);
            progress = 25; stage = "Сортировка фиксаций по функциональным зонам";
            List<FAOIsOnTimeRecord> fAOIsOnTimeRecords = RawDataProcessorMethods.ConvertTobiiCSVRecord_To_FAOIsOnTimeRecord(tobiiCSVRecords,
                                                                                                                            SourceData, ref progress, 10);
            progress = 35; stage = "Сжатие сырых данных";
            Results.TobiiCSVRecordsList = RawDataProcessorMethods.CompactTobiiCSVRecords(tobiiCSVRecords, ref progress, 5);

            progress = 40; stage = "Сжатие проанализированных данных";
            fAOIsOnTimeRecords = RawDataProcessorMethods.CompactFAOIsOnTimeRecord(fAOIsOnTimeRecords, ref progress, 5);

            progress = 45; stage = "Конвертирование результатов анализа";
            Results.FAOIHitsOnTimeIntervalList = RawDataProcessorMethods.ConvertFAOIsOnTimeRecord_to_FAOIHitsOnTimeInterval(fAOIsOnTimeRecords, ref progress, 3);

            progress = 48; stage = "Считывание данных о диаметре зрачка";

            var PupilDiameter = RawDataProcessorMethods.ReadMultipleOnTimeDistributedParameter(SourceData,
                new Column[] { SourceData.OptionalDataCSVColumns["Pupil diameter left"], SourceData.OptionalDataCSVColumns["Pupil diameter right"] }
                , "Pupil diameter", ref progress, 52, countStringsForReading + 100);
            RawDataProcessorMethods.ClearFromZeroesMultipleOnTimeDistributedParameter(PupilDiameter, 2);
            Results.PupilDiameter = RawDataProcessorMethods.CompressMultipleOnTimeDistributedParameter(PupilDiameter, 2, 2);
            progress = 100; stage = "Анализ данных завершен";
        }
        #endregion


    }

    /// <summary>
    /// Класс содержит статические функции для считывания и преобразования данных
    /// </summary>
    public static class RawDataProcessorMethods
    {
        public static List<TobiiCSVRecord> TobiiCSVRead(ProcessingTaskSourceData SourceData,
                                                        ref double progress,
                                                        double progress_koef,
                                                        long countStringsForReading = 1,
                                                        char separator = '\n',
                                                        char delimiter = '\t',
                                                        long bufferStringsSize = 10000)
        {
            long i = 0;
            List<TobiiCSVRecord> tobiiList = new List<TobiiCSVRecord>();
            using (StreamReader rd = new StreamReader(new FileStream(SourceData.CSVFileName, FileMode.Open)))
            {
                bool EndOfFile = false;
                long CountReadedStrings = 0;

                double initialProgress = progress;

                string[] first_string_arr = rd.ReadLine().Split(delimiter); //читаем первую строку - по сути нам просто надо курсор на вторую переместить.

                if (bufferStringsSize > countStringsForReading) bufferStringsSize = countStringsForReading;
                while ((!EndOfFile) && (!(CountReadedStrings >= countStringsForReading)))
                {
                    progress = initialProgress + progress_koef * ((double)CountReadedStrings / (double)countStringsForReading);
                    if (bufferStringsSize > (countStringsForReading - CountReadedStrings)) bufferStringsSize = (countStringsForReading - CountReadedStrings);//TODO:ПРоверить - тут может быть на 1 больше или меньше надо
                    string[] str_arr_tmp = { "" };
                    string big_str = "";
                    EndOfFile = CSVReader.ReadPartOfFile(rd, out big_str, bufferStringsSize);
                    str_arr_tmp = big_str.Split(separator);

                    foreach (string s in str_arr_tmp)
                    {
                        string[] tmp = { "" };
                        i++;
                        tmp = s.Split(delimiter);
                        if (tmp.Count() < 3) continue;
                        TobiiCSVRecord TR = new TobiiCSVRecord();
                        if (!long.TryParse(tmp[SourceData.CSVTimeColumn.OrderedNumber], out TR.time_ms))
                            throw new Exception("Не могу преобразовать в timestamp строку  " + tmp[SourceData.CSVTimeColumn.OrderedNumber]);

                        string[] Hits = new string[SourceData.CSVAOIHitsColumns.Count()];
                        int HitsIndex = 0;
                        foreach (var HitColumn in SourceData.CSVAOIHitsColumns)
                            if (tmp[HitColumn.OrderedNumber] == "1")
                                TR.AOIHitsColumnsInCSVFile.Add(HitColumn);

                        tobiiList.Add(TR);
                    }

                    //strings.AddRange(str_arr_tmp);
                    CountReadedStrings += bufferStringsSize;
                }
            }
            return tobiiList;
        }


        public static long TobiiCSVCalculateCountOfStrings(ProcessingTaskSourceData SourceData)
        {
            long CountReadedStrings = 0;
            using (StreamReader rd = new StreamReader(new FileStream(SourceData.CSVFileName, FileMode.Open)))
            {
                while (CountReadedStrings < 1_000_000_000)
                {
                    string s = rd.ReadLine();
                    if (s == null) break;
                    if (s.Replace(" ", "") == "") throw new Exception("Файл содержит пустые строки");
                    CountReadedStrings++;
                }
                if (CountReadedStrings >= 1_000_000_000) MessageBox.Show("Файл слишком большого размера");
            }
            return CountReadedStrings;
        }

        public static void TobiiCSVReadSecondString(ProcessingTaskSourceData SourceData)
        {
            string s = "";
            using (StreamReader rd = new StreamReader(new FileStream(SourceData.CSVFileName, FileMode.Open)))
            {
                s = rd.ReadLine();
                s = rd.ReadLine();
            }
            SourceData.CSVSecondString = s;
        }


        /// <summary>
        /// Преобразовывает записи TobiiCSVRecord в которых указаны колонки с AOIhits (время + AOIhits)
        /// в записи типа FAOIsOnTimeRecord в которых уже указываются FAOI (вермя + FAOIs)
        /// </summary>
        /// <param name="TobiiCSVRecords"></param>
        /// <returns></returns>
        public static List<FAOIsOnTimeRecord> ConvertTobiiCSVRecord_To_FAOIsOnTimeRecord(List<TobiiCSVRecord> TobiiCSVRecords,
                                                                                         ProcessingTaskSourceData SourceData,
                                                                                         ref double progress,
                                                                                         double progress_koef)
        {
            List<FAOIsOnTimeRecord> FAOIsOnTimeRecordsList = new List<FAOIsOnTimeRecord>();

            int curI = 0;
            double initialProgress = progress;
            int RecordsCount = TobiiCSVRecords.Count();
            foreach (var TR in TobiiCSVRecords)
            {
                curI++;
                progress = initialProgress + progress_koef * ((double)curI / (double)RecordsCount);

                FAOIsOnTimeRecord fAOIsOnTimeRecord = new FAOIsOnTimeRecord();
                fAOIsOnTimeRecord.time_ms = TR.time_ms;
                foreach (var AOIHitColumn in TR.AOIHitsColumnsInCSVFile)
                    if (SourceData.CSVColumnsToFAOIsConversionTable.ContainsKey(AOIHitColumn))
                        fAOIsOnTimeRecord.FAOIs.Add(SourceData.CSVColumnsToFAOIsConversionTable[AOIHitColumn]);
                fAOIsOnTimeRecord.FAOIs = fAOIsOnTimeRecord.FAOIs.Distinct().ToList();
                fAOIsOnTimeRecord.FAOIs.Sort();
                FAOIsOnTimeRecordsList.Add(fAOIsOnTimeRecord);
            }

            //добавляем последнюю запись - чтобы интервалы потом правильно считались.
            var lastRecord = new FAOIsOnTimeRecord() { time_ms = FAOIsOnTimeRecordsList.Last().time_ms + 1 };
            FAOIsOnTimeRecordsList.Add(lastRecord);

            return FAOIsOnTimeRecordsList;
        }

        /// <summary>
        /// Убираем повторы из списка записей FAOIsOnTimeRecord - компактифицируем его
        /// </summary>
        /// <param name="FAOIsOnTimeRecords"></param>
        /// <returns></returns>
        public static List<FAOIsOnTimeRecord> CompactFAOIsOnTimeRecord(List<FAOIsOnTimeRecord> FAOIsOnTimeRecords,
                                                                       ref double progress,
                                                                       double progress_koef)
        {

            int curI = 0;
            double initialProgress = progress;
            int RecordsCount = FAOIsOnTimeRecords.Count();

            List<FAOIsOnTimeRecord> RecordsNew = new List<FAOIsOnTimeRecord>();
            List<FAOI> FAOIsBefore = FAOIsOnTimeRecords[0].FAOIs;
            RecordsNew.Add(FAOIsOnTimeRecords[0]);
            for (int i = 1; i < FAOIsOnTimeRecords.Count(); i++)
            {
                curI++;
                progress = initialProgress + progress_koef * ((double)curI / (double)RecordsCount);

                var record = FAOIsOnTimeRecords[i];
                if (!IsListEqual(record.FAOIs, FAOIsBefore))
                {
                    RecordsNew.Add(record);
                    FAOIsBefore = record.FAOIs;
                }
            }
            //добавляем последнюю запись - чтобы интервалы потом правильно считались.
            //var lastRecord = new FAOIsOnTimeRecord() { time_ms = FAOIsOnTimeRecords.Last().time_ms, FAOIs = new List<FAOI>()};
            //RecordsNew.Add(lastRecord);

            return RecordsNew;
        }

        /// <summary>
        /// Убираем повторы из списка записей TobiiCSVRecords - компактифицируем его
        /// </summary>
        /// <param name="TobiiCSVRecords"></param>
        /// <returns></returns>
        public static List<TobiiCSVRecord> CompactTobiiCSVRecords(List<TobiiCSVRecord> TobiiCSVRecords,
                                                                       ref double progress,
                                                                       double progress_koef)
        {
            int curI = 0;
            double initialProgress = progress;
            int RecordsCount = TobiiCSVRecords.Count();

            List<TobiiCSVRecord> RecordsNew = new List<TobiiCSVRecord>();
            List<Column> AOIHitssBefore = TobiiCSVRecords[0].AOIHitsColumnsInCSVFile;
            RecordsNew.Add(TobiiCSVRecords[0]);
            for (int i = 1; i < TobiiCSVRecords.Count(); i++)
            {
                curI++;
                progress = initialProgress + progress_koef * ((double)curI / (double)RecordsCount);

                var record = TobiiCSVRecords[i];
                if (!IsListEqual(record.AOIHitsColumnsInCSVFile, AOIHitssBefore))
                {
                    RecordsNew.Add(record);
                    AOIHitssBefore = record.AOIHitsColumnsInCSVFile;
                }
            }
            //добавляем последнюю запись - чтобы интервалы потом правильно считались.
            RecordsNew.Add(new TobiiCSVRecord() { time_ms = TobiiCSVRecords.Last().time_ms + 1 });

            return RecordsNew;
        }


        /// <summary>
        /// Проверка эквивалентности двух списков - немного иная логика по сравнению с SequenceEqual. 
        /// Не выдает исключений если в списках нет элементов. Если в обоих списках нет элементов - считается что списки идентичны. 
        /// TODO:Эм... возможно SequenceEqual так и работает? надо бы проверить позже.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List1"></param>
        /// <param name="List2"></param>
        /// <returns></returns>
        private static bool IsListEqual<T>(List<T> List1, List<T> List2)
        {
            if (List1.Count() == 0 && List2.Count() == 0) return true;
            if (List1.Count() == 0) return false;
            if (List2.Count() == 0) return false;
            return List1.SequenceEqual(List2);
        }


        /// <summary>
        /// Преобразовывает записи с ОТМЕТКАМИ О НАЧАЛЕ времени и FAOI в записи с ИНТЕРВАЛАМИ времени и соответствующими попаданиями взгляда в FAOI
        /// </summary>
        /// <param name="FAOIsOnTimeRecords"></param>
        /// <returns></returns>
        public static List<FAOIHitsOnTimeInterval> ConvertFAOIsOnTimeRecord_to_FAOIHitsOnTimeInterval(List<FAOIsOnTimeRecord> FAOIsOnTimeRecords,
                                                                       ref double progress,
                                                                       double progress_koef)
        {
            int curI = 0;
            double initialProgress = progress;
            int RecordsCount = FAOIsOnTimeRecords.Count();

            List<FAOIHitsOnTimeInterval> RecordsNew = new List<FAOIHitsOnTimeInterval>();

            for (int i = 0; i < FAOIsOnTimeRecords.Count() - 1; i++)
            {
                curI++;
                progress = initialProgress + progress_koef * ((double)curI / (double)RecordsCount);

                TimeInterval timeInterval = new TimeInterval(TimeSpan.FromMilliseconds(FAOIsOnTimeRecords[i].time_ms), TimeSpan.FromMilliseconds(FAOIsOnTimeRecords[i + 1].time_ms));
                var record = new FAOIHitsOnTimeInterval(timeInterval, FAOIsOnTimeRecords[i].FAOIs);
                RecordsNew.Add(record);
            }



            return RecordsNew;
        }
        /// <summary>
        /// Проверяет временную целостность полученного списка записей
        /// </summary>
        /// <param name="tobiiCSVRecords"></param>
        /// <param name="progress"></param>
        /// <param name="progress_koef"></param>
        /// <returns>Возвращает true если каждая последующая запись содержит отметку времени большую чем предыдущая</returns>
        internal static bool CheckTimeIntegrity(List<TobiiCSVRecord> tobiiCSVRecords, ref double progress, double progress_koef)
        {
            int curI = 0;
            double initialProgress = progress;
            int RecordsCount = tobiiCSVRecords.Count() - 2;

            for (int i = 0; i < tobiiCSVRecords.Count - 2; i++)
            {
                curI++;
                progress = initialProgress + progress_koef * ((double)curI / (double)RecordsCount);
                if (tobiiCSVRecords[i + 1].time_ms < tobiiCSVRecords[i].time_ms)
                    return false;
            }
            return true;
        }


        /// <summary>
        /// Считывает данные о диаметре левого зрачка
        /// </summary>
        /// <returns></returns>
        internal static MultipleOnTimeDistributedParameters<double> ReadMultipleOnTimeDistributedParameter(ProcessingTaskSourceData SourceData,
                                                        Column[] csvcolumns,
                                                        string parameterName,
                                                        ref double progress,
                                                        double progress_koef,
                                                        long countStringsForReading = 1,
                                                        char separator = '\n',
                                                        char delimiter = '\t',
                                                        long bufferStringsSize = 10000)
        {
            long i = 0;
            MultipleOnTimeDistributedParameters<double> parameterList = new MultipleOnTimeDistributedParameters<double>(parameterName);
            using (StreamReader rd = new StreamReader(new FileStream(SourceData.CSVFileName, FileMode.Open)))
            {
                bool EndOfFile = false;
                long CountReadedStrings = 0;

                double initialProgress = progress;

                string[] first_string_arr = rd.ReadLine().Split(delimiter); //читаем первую строку - по сути нам просто надо курсор на вторую переместить.

                if (bufferStringsSize > countStringsForReading) bufferStringsSize = countStringsForReading;
                while ((!EndOfFile) && (!(CountReadedStrings >= countStringsForReading)))
                {
                    progress = initialProgress + progress_koef * ((double)CountReadedStrings / (double)countStringsForReading);
                    if (bufferStringsSize > (countStringsForReading - CountReadedStrings)) bufferStringsSize = (countStringsForReading - CountReadedStrings);//TODO:ПРоверить - тут может быть на 1 больше или меньше надо
                    string[] str_arr_tmp = { "" };
                    string big_str = "";
                    EndOfFile = CSVReader.ReadPartOfFile(rd, out big_str, bufferStringsSize);
                    str_arr_tmp = big_str.Split(separator);

                    foreach (string s in str_arr_tmp)
                    {
                        var result = new TimeSpan_MultipleValue_Pair<double>();
                        long tmp_time;
                        string[] tmp = { "" };
                        i++;
                        tmp = s.Split(delimiter);
                        if (tmp.Count() < 3) continue;
                        if (!long.TryParse(tmp[SourceData.CSVTimeColumn.OrderedNumber], out tmp_time))
                            throw new Exception("Не могу преобразовать в timestamp строку  " + tmp[SourceData.CSVTimeColumn.OrderedNumber]);
                        result.Time = TimeSpan.FromMilliseconds(tmp_time);

                        foreach (var column in csvcolumns)
                        {
                            double value;
                            if (!double.TryParse(tmp[column.OrderedNumber], out value))
                                value = 0;
                            result.Values.Add(value);

                        }
                        parameterList.Results.Add(result);
                    }
                    CountReadedStrings += bufferStringsSize;
                }
            }
            return parameterList;
        }

        /// <summary>
        /// Очищает коллекцию results внутри source от нулевых параметров (если параметр = 0, то берется его предыдущее значение)
        /// При этом надо указать точную гарантированную размерность внутреннего "кортежа" Values
        /// </summary>
        /// <param name="source"></param>
        /// <param name="garantedLenghtOfResultValues"></param>
        /// <returns></returns>
        internal static void ClearFromZeroesMultipleOnTimeDistributedParameter(MultipleOnTimeDistributedParameters<double> source,
            int garantedLenghtOfResultValues)
        {
            if (source.Results.Count() < 1) return;
            var prevResult = source.Results[0];
            foreach (var result in source.Results)
            {
                for (int i = 0; i < garantedLenghtOfResultValues; i++)
                {
                    try
                    {
                        if (result.Values[i] == 0)
                        {
                            result.Values[i] = prevResult.Values[i];
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                prevResult = result;
            }
        }

        /// <summary>
        /// Сжимает переданную коллекцию множества параметров распределения по времени по частоте оцифровки (Гц)
        /// </summary>
        internal static MultipleOnTimeDistributedParameters<double> CompressMultipleOnTimeDistributedParameter(MultipleOnTimeDistributedParameters<double> sourceCollection,
            double frequencyHz, int garantedLenghtOfResultValues)
        {
            MultipleOnTimeDistributedParameters<double> filtredCollection = new MultipleOnTimeDistributedParameters<double>(sourceCollection.ParameterName);
            TimeSpan period = TimeSpan.FromSeconds(1 / frequencyHz);
            TimeSpan sourceBegin = sourceCollection.Results[0].Time;
            TimeSpan sourceEnd = sourceCollection.Results.Last().Time;
            TimeSpan curtime = sourceBegin;
            int i = 0;
            while (curtime < sourceEnd)
            {
                List<double> newValues = new List<double>();
                var IntervalBegin = curtime;
                var IntervalEnd = curtime + period;

                List<double> summs = new List<double>();
                for (int valIndex = 0; valIndex < garantedLenghtOfResultValues; valIndex++)
                {
                    summs.Add(0);
                }
                int count = 0;

                while (sourceCollection.Results[i].Time < IntervalEnd)
                {
                    for (int valIndex = 0; valIndex < garantedLenghtOfResultValues; valIndex++)
                    {
                        summs[valIndex] += sourceCollection.Results[i].Values[valIndex];
                    }
                    count++;
                    i++;
                    if (i >= sourceCollection.Results.Count()) break;
                }
                for (int valIndex = 0; valIndex < garantedLenghtOfResultValues; valIndex++)
                {
                    newValues.Add(summs[valIndex] / (double)count);//по моему тут явное приведение не нужно
                }
                filtredCollection.Results.Add(new TimeSpan_MultipleValue_Pair<double>() { Time = IntervalBegin, Values = newValues });
                curtime = IntervalEnd;
            }

            return filtredCollection;
        }
    }

}
