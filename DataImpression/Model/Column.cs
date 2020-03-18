using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    //Проверка
    //Предлагаю удалить эту проверку. Тест code-review в ветке Prototype1_tests

    /// <summary>
    /// Столбец
    /// </summary>
    public class Column
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
