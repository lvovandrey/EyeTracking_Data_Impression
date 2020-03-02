using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Model
{
    /// <summary>
    /// Функциональная зона досягаемости (Functional Area of Interests)
    /// </summary>
    class FAOI
    {
        #region ctor
        public FAOI(int orderedNumber, string name)
        {
            OrderedNumber = orderedNumber;
            Name = name;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Порядковый номер зоны
        /// </summary>
        int OrderedNumber { get; set; }

        /// <summary>
        /// Название зоны
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Methods
        #endregion

    }
}
