using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PupilDiameterControlLibrary
{
    /// <summary>
    /// Класс объединяет время и оба диаметра зрачков
    /// </summary>
    public class TimeSpan_PupilDiameters_Pair
    {
        public TimeSpan_PupilDiameters_Pair()
        {
           
        }
        public double PupilDiameterLeft { get; set; }
        public double PupilDiameterRight { get; set; }
        public TimeSpan Time { get; set; }
    }
}
