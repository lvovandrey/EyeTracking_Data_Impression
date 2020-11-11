using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImpression.Models.Helpers
{
    public static class Logger
    {
        private static string logFileName 
        {
            get { return Path.Combine(Environment.CurrentDirectory,"log.txt"); }
        }
        public static void Write(string text)
        {
            using (StreamWriter sw = new StreamWriter(logFileName, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(DateTime.Now.ToString() + "\t" + Environment.MachineName+ "\t" +  text);
                Console.WriteLine(text);
            }
        }
    }
}
