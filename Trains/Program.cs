using System;
using Trains.Utils;

namespace Trains
{
    class Program
    {
        static void Main(string[] args)
        {
            string key = "Y";
            do
            {
                try
                {
                    Console.WriteLine("Please input the text file path \n(Pressing enter directly will load the default graph data) ");
                    Console.Write(@"File Path:");
                    var path = Console.ReadLine();
                    //通过绝对路劲获取graph
                    var graphData = FileUtils.GetTxtDataByPath(path);
                    if (!string.IsNullOrEmpty(graphData))
                    {
                        Console.WriteLine(string.Format(@"Graph: {0}", graphData));
                        //构造图
                        var graph = new Graph(graphData);
                        //铁路系统工具类
                        RailroadUtils server = new RailroadUtils(graphData);
                        server.TestOutput();
                    }
                    Console.Write("\nContinue(Y/N)?:");
                    key = Console.ReadLine().Trim();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.Write("\nContinue(Y/N)?:");
                    key = Console.ReadLine().Trim();
                }
                Console.WriteLine();
            } while (key.ToUpper().Equals("Y"));
            Console.Read();
        }
    }
}
