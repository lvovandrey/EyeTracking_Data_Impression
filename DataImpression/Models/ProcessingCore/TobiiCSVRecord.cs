using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    public class TobiiCSVRecord
    {
       
            public long time_ms;
            public List<Column> AOIHitsColumnsInCSVFile = new List<Column>();

            public TobiiCSVRecord()
            {
                time_ms = 0;
            }

            public TobiiCSVRecord(TobiiCSVRecord TR)
            {
                time_ms = TR.time_ms;
            }
        
    }

    public class FAOIsOnTimeRecord
    {

        public long time_ms;
        public List<FAOI> FAOIs = new List<FAOI>();

        public FAOIsOnTimeRecord()
        {
            time_ms = 0;
        }

        public FAOIsOnTimeRecord(FAOIsOnTimeRecord Record)
        {
            time_ms = Record.time_ms;
        }

    }
}
