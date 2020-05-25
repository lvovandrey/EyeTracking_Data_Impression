using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    class CSVReader
    {
        public void TobiiCSVRead(string filename, List<TobiiRecord> tobiiList, int ZoneColCount)
        {

            char separator = '\n';
            char delimiter = '\t';

            int N_timestampCol = 0, N_firstZoneCol = 0;
            long i = 0;
            using (StreamReader rd = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                string[] first_string_arr = { "" };
                first_string_arr = rd.ReadLine().Split(delimiter);
                N_timestampCol = SearchColFirst(first_string_arr, "Recording timestamp");
                N_firstZoneCol = SearchColFirst(first_string_arr, "AOI hit [");

                bool EndOfFile = false;
                while (!EndOfFile)
                {
                    string[] str_arr = { "" };
                    string big_str = "";
                    EndOfFile = ReadPartOfFile(rd, out big_str);

                    str_arr = big_str.Split(separator);
                    foreach (string s in str_arr)
                    {
                        string[] tmp = { "" };
                        i++;
                        tmp = s.Split(delimiter);
                        if (tmp.Count() < 3) continue;
                        TobiiRecord TR = new TobiiRecord();
                        if (!long.TryParse(tmp[N_timestampCol], out TR.time_ms))
                            throw new Exception("Не могу преобразовать в timestamp строку  " + tmp[N_timestampCol]);

                        string[] Hits = new string[tmp.Count()];
                        try
                        {
                            Array.Copy(tmp, N_firstZoneCol, Hits, 0, ZoneColCount);
                        }
                        catch
                        { Console.WriteLine("!!!"); }
                        TR.zones = SearchCol(Hits, "1");
                        tobiiList.Add(TR);
                    }

                }

                FiltredTobiiList = CompactTobiiRecords(tobiiList);
            }
        }
        public void TobiiCSVRead(string filename, List<TobiiRecord> tobiiList, int ZoneColCount)
        {

            char separator = '\n';
            char delimiter = '\t';

            int N_timestampCol = 0, N_firstZoneCol = 0;
            long i = 0;
            using (StreamReader rd = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                string[] first_string_arr = { "" };
                first_string_arr = rd.ReadLine().Split(delimiter);
                N_timestampCol = SearchColFirst(first_string_arr, "Recording timestamp");
                N_firstZoneCol = SearchColFirst(first_string_arr, "AOI hit [");

                bool EndOfFile = false;
                while (!EndOfFile)
                {
                    string[] str_arr = { "" };
                    string big_str = "";
                    EndOfFile = ReadPartOfFile(rd, out big_str);

                    str_arr = big_str.Split(separator);
                    foreach (string s in str_arr)
                    {
                        string[] tmp = { "" };
                        i++;
                        tmp = s.Split(delimiter);
                        if (tmp.Count() < 3) continue;
                        TobiiRecord TR = new TobiiRecord();
                        if (!long.TryParse(tmp[N_timestampCol], out TR.time_ms))
                            throw new Exception("Не могу преобразовать в timestamp строку  " + tmp[N_timestampCol]);

                        string[] Hits = new string[tmp.Count()];
                        try
                        {
                            Array.Copy(tmp, N_firstZoneCol, Hits, 0, ZoneColCount);
                        }
                        catch
                        { Console.WriteLine("!!!"); }
                        TR.zones = SearchCol(Hits, "1");
                        tobiiList.Add(TR);
                    }

                }

                FiltredTobiiList = CompactTobiiRecords(tobiiList);
            }
        }
    }
}
