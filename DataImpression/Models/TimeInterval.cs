using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Временной интервал.
    /// </summary>
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
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Граница начала временного интервала
        /// </summary>
        public TimeSpan TimeBegin { get; set;}

        /// <summary>
        /// Граница конца временного интервала
        /// </summary>
        public TimeSpan TimeEnd { get; set; }

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
