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
    public class FAOI:IComparable, IComparable<FAOI>
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
        public int CompareTo(object obj)
        {
            if (obj is FAOI)
                return (this.OrderedNumber - ((FAOI)obj).OrderedNumber);
            return -1;
        }

        public int CompareTo(FAOI other)
        {
            return (this.OrderedNumber - other.OrderedNumber);
        }
        #endregion

    }
}
