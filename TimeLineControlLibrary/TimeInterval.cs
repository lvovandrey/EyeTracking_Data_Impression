using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLineControlLibrary
{
    public struct TimeInterval
    {
        public TimeSpan Begin;
        public TimeSpan End;
        public TimeSpan Duration => End - Begin;
    }
}
