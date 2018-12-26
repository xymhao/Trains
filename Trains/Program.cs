using System;
using System.IO;
using Trains.Utils;

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
                var graphData = FileUtils.GetTxtDataByPath(path);
                var graph = new Graph(graphData);
                RailroadUtils server = new RailroadUtils(graph);
                server.TestOutput();
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
