using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Trains.Utils
{
    public class FileUtils
    {
        public static string GetTxtDataByPath(string path)
        {
            string data = string.Empty;

            if (string.IsNullOrEmpty(path))
            {
                path = Path.GetFullPath(Constant.DEFAULT_GRAPH);
            }

            using (StreamReader sr = File.OpenText(path))
            {
                string fileStr = string.Empty;
                while ((fileStr = sr.ReadLine()) != null)
                {
                    data += fileStr;
                }
            }
            Console.WriteLine(string.Format(@"The graph is {0}", data));
            return data;
        }
    }
}
