using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DataImpression.Models
{
    /// <summary>
    /// Временной интервал.
    /// </summary>
    [Serializable]
    public class TimeInterval
    {
        #region ctor
        public TimeInterval(TimeSpan timeBegin, TimeSpan timeEnd)
        {
            if (timeBegin > timeEnd) //вадидация данных
                throw new ArgumentException("ctor TimeInterval: Argument timeBegin must be less than timeEnd");

            TimeBegin = timeBegin;
            TimeEnd = timeEnd;
        }
        public TimeInterval()
        {
            //TimeBegin = TimeSpan.Zero;
            //TimeEnd = TimeSpan.FromMilliseconds(1);
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        public long timeBegin_ms;
        /// <summary>
        /// Граница начала временного интервала
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeBegin { get { return TimeSpan.FromMilliseconds(timeBegin_ms); } set { timeBegin_ms = (long)value.TotalMilliseconds; } }

        public long timeEnd_ms;
        /// <summary>
        /// Граница конца временного интервала
        /// </summary>
        [XmlIgnore]
        public TimeSpan TimeEnd { get { return TimeSpan.FromMilliseconds(timeEnd_ms); } set { timeEnd_ms = (long)value.TotalMilliseconds; } }

        #endregion

        #region Methods
        /// <summary>
        /// Длительность временного интервала
        /// </summary>
        /// <returns></returns>
        public TimeSpan Duration()
        {
            return TimeEnd - TimeBegin;
        }
        #endregion
    }
}
