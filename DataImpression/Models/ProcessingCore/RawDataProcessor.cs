using System;
using System.Collections.Generic;
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
        public RawDataProcessor(ProcessingTaskSourceData sourceData)
        {
            SourceData = sourceData;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Источник данных
        /// </summary>
        public ProcessingTaskSourceData SourceData { get; set; }

        #endregion

        #region Methods
        public void ConvertCSVRawDataToFAOIHitsOnTimeIntervalList()
        {
            if (SourceData.CSVAOIHitsColumns == null) throw new Exception("Incomplete data: SourceData.CSVAOIHitsColumns");
            if (SourceData.WorkingTOI == null) throw new Exception("Incomplete data: SourceData.WorkingTOI");
            if (SourceData.FAOIs == null) throw new Exception("Incomplete data: SourceData.FAOIs");
            if (SourceData.CSVColumnsToFAOIsConversionTable == null) throw new Exception("Incomplete data: SourceData.CSVColumnsToFAOIsConversionTable");
            if (SourceData.CSVTimeColumn == null) throw new Exception("Incomplete data: SourceData.CSVTimeColumn");
            if (SourceData.CSVRawData == null) throw new Exception("Incomplete data: SourceData.CSVRawData");

            //TODO: тут должна быть обработка
        }
        #endregion


    }
}
