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
    [Serializable]
    public class Column
    {
        #region ctor
        public Column(int orderedNumber, string name)
        {
            OrderedNumber = orderedNumber;
            Name = name;
        }
        public Column()
        {
            OrderedNumber = 0;
            Name = "";
        }
        #endregion

        #region Fields
        #endregion

        #region Properties
        /// <summary>
        /// Порядковый номер столбца. Номера начинаются с 0
        /// </summary>
        public int OrderedNumber { get; set; }

        /// <summary>
        /// Название столбца
        /// </summary>
        public string Name { get; set; }
        #endregion

        #region Methods
        public static List<Column> ToColumns(List<string> strings)
        {
            List<Column> columns = new List<Column>();
            for( int i = 0; i<strings.Count(); i++) 
                columns.Add(new Column(i, strings[i]));

            return columns;
        }
        #endregion
    }
}
