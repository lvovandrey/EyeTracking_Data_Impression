using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models.ResultTypes
{
    public class FAOIDistributed_Parameter<T>
    {
        public string ParameterName;
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
