using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Класс хранит в себе данные одной записи (строки) из csv-файла в формате tobii
    /// </summary>
    [Serializable]
    public class TobiiCSVRecord
    {
        /// <summary>
        /// Время в милисекундах
        /// </summary>
        public long time_ms;

        /// <summary>
        /// Список зон AOI куда испытуемый смотрит в этот момент времени
        /// </summary>
        public List<Column> AOIHitsColumnsInCSVFile = new List<Column>();

        public TobiiCSVRecord()
        {
            time_ms = 0;
        }

        public TobiiCSVRecord(TobiiCSVRecord TR)
        {
            time_ms = TR.time_ms;
        }

    }


    
}
