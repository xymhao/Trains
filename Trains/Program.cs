using System;
using System.IO;

namespace Trains
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(@"Please input text file fullPath(If input empty that will load default file)");
                Console.Write(@"File Path:");
                var path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.GetFullPath("graph.txt");
                }

                using (StreamReader sr = File.OpenText(path))
                {
                    string graphStr = string.Empty;
                    string fileStr = string.Empty;
                    while ((fileStr = sr.ReadLine()) != null)
                    {
                        graphStr += fileStr;
                    }
                    Console.WriteLine(string.Format(@"The graph is {0}", graphStr));
                    var graph = new Graph(graphStr);
                    RailroadServices server = new RailroadServices(graph);
                    server.TestOutput();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Please restart the application!");
            }
            Console.Read();
            Console.WriteLine("Hello World!");
        }
    }
}
