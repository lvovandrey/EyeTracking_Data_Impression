using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models.ResultTypes
{

    /// <summary>
    /// Класс содержит коллекцию результатов для нескольких связанных параметров, имещих распределение по времени. (например зависимость диаметров двух зрачков от времени)
    /// При этом параметры должны быть связанными (т.е. одному времени должны соответствовать два или более параметра всегда)
    /// </summary>
    /// <typeparam name="T">Тип параметра, то есть буквально - экземпляр этого класса содержит распределение параметра по времени. 
    /// Тип этого параметра и есть этот закрывающий тип. 
    /// В основном конечно будут параметры числовые с плавающей точкой, но возможны разные извращения.</typeparam>
    public class MultipleOnTimeDistributedParameters<T>
    {
        /// <summary>
        /// Имя параметра (например, "Диаметр зрачка")
        /// </summary>
        public string ParameterName;

        /// <summary>
        /// Коллекция результатов, содержащая как раз это самое распределение по времени.
        /// </summary>
        public List<TimeSpan_MultipleValue_Pair<T>> Results = new List<TimeSpan_MultipleValue_Pair<T>>();

        public MultipleOnTimeDistributedParameters(string parameterName)
        {
            ParameterName = parameterName;
        }
        public MultipleOnTimeDistributedParameters()
        {
            ParameterName = "No name";
        }

    }

    /// <summary>
    /// Пара "время- Набор значений параметра"
    /// </summary>
    public class TimeSpan_MultipleValue_Pair<T>
    {
        public TimeSpan_MultipleValue_Pair()
        {
            Values = new List<T>();
        }
        public List<T> Values { get; set; }
        public TimeSpan Time { get; set; }

    }
}
