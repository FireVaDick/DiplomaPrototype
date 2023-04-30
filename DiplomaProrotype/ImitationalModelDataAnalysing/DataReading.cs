using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaPrototype.ImitationalModelDataAnalysing
{
    internal class DataReading
    {
        public static string routes = string.Empty;
        public static void Read()
        {
            StreamReader streamReader = new StreamReader("Model.txt");
            while (!streamReader.EndOfStream)
            {
                routes += streamReader.ReadLine();
                routes += '\n';
            }
            streamReader.Close();
        }
    }
}
