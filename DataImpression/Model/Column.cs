using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Model
{
    //Проверка
    /// <summary>
    /// Столбец
    /// </summary>
    class Column
    {
        #region ctor
        public Column(int orderedNumber, string name)
        {
            OrderedNumber = orderedNumber;
            Name = name;
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Порядковый номер столбца
        /// </summary>
        int OrderedNumber { get; set; }

        /// <summary>
        /// Название столбца
        /// </summary>
        string Name { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
