using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Временная зона - отрезок времени из полного временного интервала.
    /// Под этим можно понимать например часть времени, которая анализируется из всей записи.
    /// </summary>
    [Serializable]
    public class TOI
    {
        #region ctor
        public TOI(TimeInterval workingTimeInterval, TimeInterval fullTimeInterval)
        {
            WorkingTimeInterval = workingTimeInterval;//TODO: Валидация данных - если workingTimeInterval вне fullTimeInterval - кинуть Exception
            FullTimeInterval = fullTimeInterval;
        }

        public TOI()
        {
            WorkingTimeInterval = new TimeInterval();//TODO: Валидация данных - если workingTimeInterval вне fullTimeInterval - кинуть Exception
            FullTimeInterval = new TimeInterval();
        }

        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Рабочий интервал времени - т.е. тот с которым непосредственно будет производиться работа, например, на нем будут анализироваться данные
        /// </summary>
        TimeInterval WorkingTimeInterval { get; set; }

        /// <summary>
        /// Полный интервал времени - т.е. вообще весь временной отрезок, внутри которого находится
        /// </summary>
        TimeInterval FullTimeInterval { get; set; }
        #endregion

        #region Methods
        #endregion

    }
}
