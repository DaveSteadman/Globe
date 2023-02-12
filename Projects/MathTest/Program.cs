using DotNetMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("- - - MathTest - - -");

            LLAPos p1 = new LLAPos(0, 0, 0);
            LLAPos p2 = new LLAPos(0, 0, 0);
            double dist = p1.DistanceToM(p2);

            Console.WriteLine("- - - Done - - -");
            Console.ReadKey();
        }
    }
}
