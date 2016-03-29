using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPR.lb1
{
    class Program
    {
        static void Main(string[] args)
        {
            // x , y coords
            float x = -1.2f,
                  y = 1,
                  sideLength = 30,
                  minSideLength = 0.000001f,
                  deltaSideLenth = 1.1f; // reduce side length 
            int iterationCount = 200,
                innerPointsCount = 1400; // count of gennereted points 

            SquarePoint startPoint = new SquarePoint(x,y);
            HyperCube HC = new HyperCube(startPoint, sideLength, minSideLength, deltaSideLenth, iterationCount, innerPointsCount); // Create Hyper cube
            var point = HC.Calculate();
            Console.ReadKey();
        }
    }
}
