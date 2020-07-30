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
    public class Column : IComparable, IComparable<Column>
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
            for (int i = 0; i < strings.Count(); i++)
                columns.Add(new Column(i, strings[i]));

            return columns;
        }

        /// <summary>
        /// Переопределяем для правильной работы механизмов сравнения 
        /// (например, если будем использовать в коллекции типа Dictionary без этого разные объекты с одинаковым содержанием нельзя будет использовать как один и тот же ключ)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj.ToString().Equals(ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}-{1} column", OrderedNumber, Name);
        }

        /// <summary>
        /// Осуществляем сортировку - по умолчанию сортировка происходит по порядковому номеру   
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int IComparable<Column>.CompareTo(Column other)
        {
            return (this.OrderedNumber - other.OrderedNumber);
        }

        public int CompareTo(object obj)
        {
            if(obj is Column)
                return (this.OrderedNumber - ((Column)obj).OrderedNumber);
            return -1;
        }
        #endregion
    }
}
