using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models.ProcessingCore
{
    /// <summary>
    /// Класс содержит запись данных о том в какие функциональные зоны FAOI испытуемый смотрит в указанный момент времени (time_ms)
    /// </summary>
    public class FAOIsOnTimeRecord
    {

        /// <summary>
        /// Время в мсек
        /// </summary>
        public long time_ms;

        /// <summary>
        /// список функциональных зон, куда в указанный момент времени смотрел испытуемый
        /// </summary>
        public List<FAOI> FAOIs = new List<FAOI>();

        public FAOIsOnTimeRecord()
        {
            time_ms = 0;
        }

        public FAOIsOnTimeRecord(FAOIsOnTimeRecord Record)
        {
            time_ms = Record.time_ms;
        }

    }
}
