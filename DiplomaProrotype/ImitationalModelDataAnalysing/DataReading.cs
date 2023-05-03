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
        public static List<string> routes = new List<string>();
        public static void Read()
        {
            routes.Clear();

            StreamReader streamReader = new StreamReader("Model.txt");
            while (!streamReader.EndOfStream)
            {
                routes.Add(streamReader.ReadLine());
            }
            streamReader.Close();
        }
    }
}
