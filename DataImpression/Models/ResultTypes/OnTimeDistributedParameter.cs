using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models.ResultTypes
{
    /// <summary>
    /// Класс содержит коллекцию результатов для какого-либо параметра, имещюего распределение по времени. (например зависимость диаметра зрачка от времени)
    /// </summary>
    /// <typeparam name="T">Тип параметра, то есть буквально - экземпляр этого класса содержит распределение параметра по времени. 
    /// Тип этого параметра и есть этот закрывающий тип. 
    /// В основном конечно будут параметры числовые с плавающей точкой, но возможны разные извращения.</typeparam>
    public class OnTimeDistributedParameter<T>
    {
        /// <summary>
        /// Имя параметра (например, "Диаметр зрачка")
        /// </summary>
        public string ParameterName;

        /// <summary>
        /// Коллекция результатов, содержащая как раз это самое распределение по времени.
        /// </summary>
        public List<TimeSpan_Value_Pair<T>> Results = new List<TimeSpan_Value_Pair<T>>();

        public OnTimeDistributedParameter(string parameterName)
        {
            ParameterName = parameterName;
        }
        public OnTimeDistributedParameter()
        {
            ParameterName = "No name";
        }

    }

    /// <summary>
    /// Пара "зона-значение параметра"
    /// </summary>
    public class TimeSpan_Value_Pair<T>
    {
        public TimeSpan_Value_Pair()
        {

        }
        public T Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
