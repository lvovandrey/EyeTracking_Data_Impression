using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models.ResultTypes
{
    /// <summary>
    /// Класс содержит коллекцию результатов для какого-либо параметра, имещюего распределение по функциональным зонам.
    /// </summary>
    /// <typeparam name="T">Тип параметра, то есть буквально - экземпляр этого класса содержит распределение параметра по зонам. Тип этого параметра и есть этот закрывающий тип. 
    /// В основном конечно будут параметры числовые с плавающей точкой, но возможны разные извращения.</typeparam>
    public class FAOIDistributed_Parameter<T>
    {
        /// <summary>
        /// Имя параметра (например, "доля времени в каждой зоне")
        /// </summary>
        public string ParameterName;
        
        /// <summary>
        /// Коллекция результатов, содержащая как раз это самое распределение по зонам.
        /// </summary>
        public List<FAOI_Value_Pair<T>> Results = new List<FAOI_Value_Pair<T>>();

        public FAOIDistributed_Parameter(string parameterName)
        {
            ParameterName = parameterName;
        }

    }

    /// <summary>
    /// Пара "зона-значение параметра"
    /// </summary>
    public class FAOI_Value_Pair<T>
    {
        public T Value { get; set; }
        public FAOI FAOI { get; set; }
    }
}
