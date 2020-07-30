using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public void ConvertCSVRawDataToFAOIHitsOnTimeIntervalList()
        {
            if (SourceData.CSVAOIHitsColumns == null) throw new Exception("Incomplete data: SourceData.CSVAOIHitsColumns");
            if (SourceData.WorkingTOI == null) throw new Exception("Incomplete data: SourceData.WorkingTOI");
            if (SourceData.FAOIs == null) throw new Exception("Incomplete data: SourceData.FAOIs");
            if (SourceData.CSVColumnsToFAOIsConversionTable == null) throw new Exception("Incomplete data: SourceData.CSVColumnsToFAOIsConversionTable");
            if (SourceData.CSVTimeColumn == null) throw new Exception("Incomplete data: SourceData.CSVTimeColumn");

            //TODO: тут должна быть обработка

            List<TobiiCSVRecord> tobiiCSVRecords = RawDataProcessorMethods.TobiiCSVRead(SourceData, 1_000_000_000);
            List<FAOIsOnTimeRecord> fAOIsOnTimeRecords = RawDataProcessorMethods.ConvertTobiiCSVRecord_To_FAOIsOnTimeRecord(tobiiCSVRecords, SourceData);
            fAOIsOnTimeRecords = RawDataProcessorMethods.CompactFAOIsOnTimeRecord(fAOIsOnTimeRecords);
        }
        #endregion


    }

    /// <summary>
    /// Класс содержит статические функции для считывания и преобразования данных
    /// </summary>
    public static class RawDataProcessorMethods
    {
        public static List<TobiiCSVRecord> TobiiCSVRead(ProcessingTaskSourceData SourceData, long countStringsForReading = 1, char separator = '\n', char delimiter = '\t', long bufferStringsSize = 10000)
        {
            long i = 0;
            List<TobiiCSVRecord> tobiiList = new List<TobiiCSVRecord>();
            using (StreamReader rd = new StreamReader(new FileStream(SourceData.CSVFileName, FileMode.Open)))
            {
                bool EndOfFile = false;
                long CountReadedStrings = 0;

                string[] first_string_arr = rd.ReadLine().Split(delimiter); //читаем первую строку - по сути нам просто надо курсор на вторую переместить.

                if (bufferStringsSize > countStringsForReading) bufferStringsSize = countStringsForReading;
                while ((!EndOfFile) && (!(CountReadedStrings >= countStringsForReading)))
                {

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

        /// <summary>
        /// Преобразовывает записи TobiiCSVRecord в которых указаны колонки с AOIhits (время + AOIhits)
        /// в записи типа FAOIsOnTimeRecord в которых уже указываются FAOI (вермя + FAOIs)
        /// </summary>
        /// <param name="TobiiCSVRecords"></param>
        /// <returns></returns>
        public static List<FAOIsOnTimeRecord> ConvertTobiiCSVRecord_To_FAOIsOnTimeRecord(List<TobiiCSVRecord> TobiiCSVRecords, ProcessingTaskSourceData SourceData)
        {
            List<FAOIsOnTimeRecord> FAOIsOnTimeRecordsList = new List<FAOIsOnTimeRecord>();
            foreach (var TR in TobiiCSVRecords)
            {
                FAOIsOnTimeRecord fAOIsOnTimeRecord = new FAOIsOnTimeRecord();
                fAOIsOnTimeRecord.time_ms = TR.time_ms;
                foreach (var AOIHitColumn in TR.AOIHitsColumnsInCSVFile)
                    if (SourceData.CSVColumnsToFAOIsConversionTable.ContainsKey(AOIHitColumn))
                        fAOIsOnTimeRecord.FAOIs.Add(SourceData.CSVColumnsToFAOIsConversionTable[AOIHitColumn]);
                fAOIsOnTimeRecord.FAOIs = fAOIsOnTimeRecord.FAOIs.Distinct().ToList();
                fAOIsOnTimeRecord.FAOIs.Sort();
                FAOIsOnTimeRecordsList.Add(fAOIsOnTimeRecord);
            }
            return FAOIsOnTimeRecordsList;
        }

        //Убираем повторы из записи тоби - компактифицируем ее
        public static List<FAOIsOnTimeRecord> CompactFAOIsOnTimeRecord(List<FAOIsOnTimeRecord> FAOIsOnTimeRecords)
        {

            List<FAOIsOnTimeRecord> RecordsNew = new List<FAOIsOnTimeRecord>();
            List<FAOI> FAOIsBefore = FAOIsOnTimeRecords[0].FAOIs;

            for (int i = 1; i < FAOIsOnTimeRecords.Count(); i++)
            {
                var record = FAOIsOnTimeRecords[i];
                if (!IsFAOIsListEqual(record.FAOIs, FAOIsBefore))
                {
                    RecordsNew.Add(record);
                    FAOIsBefore = record.FAOIs;
                }
            }
            return RecordsNew;

        }

        /// <summary>
        /// Наверное можно
        /// </summary>
        /// <param name="fAOIs"></param>
        /// <param name="fAOIsBefore"></param>
        /// <returns></returns>
        private static bool IsFAOIsListEqual(List<FAOI> fAOIs, List<FAOI> fAOIsBefore)
        {
            if (fAOIs.Count() == 0 && fAOIsBefore.Count() == 0) return true;
            if (fAOIs.Count() == 0) return false;
            if (fAOIsBefore.Count() == 0) return false;
            return fAOIs.SequenceEqual(fAOIsBefore);
        }
    }

}
