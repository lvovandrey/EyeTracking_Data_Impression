using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    /// <summary>
    /// Функциональная зона досягаемости (Functional Area of Interests)
    /// </summary>
    [Serializable]
    public class FAOI
    {
        #region ctor
        public FAOI(int orderedNumber, string name)
        {
            OrderedNumber = orderedNumber;
            Name = name;
        }
        public FAOI()
        {
            OrderedNumber = 0;
            Name = "";
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Порядковый номер зоны
        /// </summary>
        public int OrderedNumber { get; set; }

        /// <summary>
        /// Название зоны
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Methods
        #endregion

    }
}
