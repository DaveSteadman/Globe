using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace MiscTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("- - - MiscTest - - -");

            {
                GlobeConfig globeConfig = new GlobeConfig();
                globeConfig.LoadOrCreateJSONConfig("globeConfig.json");

                globeConfig.SetParam("test", "test 2023-02-11");
                if (globeConfig.HasParam("test"))
                {
                    Console.WriteLine("test: " + globeConfig.GetParam("test"));
                }
                if (globeConfig.HasParam("NeverValid"))
                {
                    Console.WriteLine("NeverValid");
                }
            }

            Console.WriteLine("- - - Done - - -");
            Console.ReadKey();
        }
    }
}
