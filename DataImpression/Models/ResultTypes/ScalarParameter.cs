using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models.ResultTypes
{
    /// <summary>
    /// Содержит один скалярный параметр (т.е. не распределенный - просто число/цифру/строку)
    /// </summary>
    /// <typeparam name="T">Тип параметра, то есть буквально - экземпляр этого класса содержит скалярную величину. Тип этого величины и есть этот закрывающий тип.</typeparam>
    public class ScalarParameter<T>
    {
        /// <summary>
        /// Имя параметра (например, "Полное количество переходов взгляда между функциональными зонами")
        /// </summary>
        public string ParameterName;

        /// <summary>
        /// Поле содержащее значение скалярного параметра.
        /// </summary>
        public T Value;

        public ScalarParameter(string parameterName, T defaultValue)
        {
            ParameterName = parameterName;
            Value = defaultValue;
        }
    }


}
