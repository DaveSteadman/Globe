using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DotNetMath;

class TestCentre
{
    public static void RunTests()
    {
        if (RunTestLLAPosBasics())
        {
            Console.WriteLine("TestCentre: RunTestLLAPosBasics() passed");
        }
        else
        {
            Console.WriteLine("TestCentre: RunTestLLAPosBasics() failed");
        }
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

    public static bool RunTestLLAPosBasics()
    {
        LLAPos pos1 = new LLAPos(10, 10);
        LLAPos pos2 = new LLAPos(12, 12);

        return true;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
}
