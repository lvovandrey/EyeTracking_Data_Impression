using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PupilDiameterControlLibrary
{
    public class TimeSpan_Value_Pair<T>
    {
        public TimeSpan_Value_Pair()
        {

        }
        public T Value { get; set; }
        public TimeSpan Time { get; set; }
    }
}
