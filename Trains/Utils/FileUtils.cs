using System;
using System.IO;

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
                path = path.Replace(@"\bin\Debug\netcoreapp2.1", "");
                Console.WriteLine($"The file path is {path}");
            }
            if (!File.Exists(path))
            {
                Console.WriteLine(@"The file path doesn't exist.");
                return string.Empty;
            }
            else
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string fileStr = string.Empty;
                    while ((fileStr = sr.ReadLine()) != null)
                    {
                        data += fileStr.Trim();
                    }
                }
            }
            return data;
        }
    }
}
