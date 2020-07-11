using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models
{
    class CSVReader
    {

        public List<string> TobiiCSVReadStrings(string filename, long countStringsForReading = 1, char separator = '\n', char delimiter = '\t', long bufferStringsSize = 10000)
        //Функция низкопроизводительная, хоть и использует буфер. На самом деле она сильно забивает память. 
        //Для нормального считывания нужно несколько изменить алгоритм и на лету обрабатывать поступающие строки а не хранить их.
        {
            long i = 0;
            List<string> strings = new List<string>();
            using (StreamReader rd = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                bool EndOfFile = false;
                long CountReadedStrings = 0;
                if (bufferStringsSize > countStringsForReading) bufferStringsSize = countStringsForReading;
                while ((!EndOfFile) && (!(CountReadedStrings >= countStringsForReading)))
                {

                    if (bufferStringsSize > (countStringsForReading - CountReadedStrings)) bufferStringsSize = (countStringsForReading - CountReadedStrings);//TODO:ПРоверить - тут может быть на 1 больше или меньше надо
                    string[] str_arr_tmp = { "" };
                    string big_str = "";
                    EndOfFile = ReadPartOfFile(rd, out big_str, bufferStringsSize);
                    str_arr_tmp = big_str.Split(separator);
                    strings.AddRange(str_arr_tmp);
                    CountReadedStrings += bufferStringsSize;
                }
            }
            return strings;
        }

        /// <summary>
        /// Чтение части файла
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="str"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static bool ReadPartOfFile(StreamReader rd, out string str, long bufferSize = 10000)
        {
            bool endOfFile = false;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bufferSize; i++)
            {
                string s = rd.ReadLine();
                if (s == null) { endOfFile = true; break; }
                sb.Append(s);
                sb.Append("\n");
            }
            str = sb.ToString();
            return endOfFile;
        }
    }
}
