using System;
using System.Collections.Generic;
using System.Linq;

namespace OPR.lb1
{
    public sealed class HyperCube
    {
        private readonly SquarePoint startPoint;
        private readonly float sideLength, iterationCount, minSideLength, deltaSideLength;
        private readonly int innerPointsCount;

        public HyperCube(
            SquarePoint startpoint,
            float sideLength,
            float minSideLength,
            float deltaSideLenth,
            int iterationCount,
            int innerPointsCount)
        {
            this.deltaSideLength = deltaSideLenth;
            this.startPoint = startpoint;
            this.sideLength = sideLength;
            this.iterationCount = iterationCount;
            this.minSideLength = minSideLength;
            this.innerPointsCount = innerPointsCount;
        }

        public SquarePoint Calculate()
        {

            SquarePoint stPoint = startPoint;
            int iteration = 0;
            float sl = this.sideLength;
            do
            {
                var square = iteration == 0
                    ? Square.GeneratePointsFromLeftCorner(stPoint, sl, innerPointsCount)
                    : Square.GeneratePointsFromCenter(stPoint, sl, innerPointsCount);

                var stPoints = square.InnerPoints
                    .Select(x => new { point = x, result = multiplicationCoord(x.x, x.y) })
                    .OrderBy(x => x.result).ToList();
                stPoint = stPoints
                    .First().point;

                Console.WriteLine("\nBest point on {0} iteration :", iteration);
                Console.WriteLine("x " + stPoint.x);
                Console.WriteLine("y " + stPoint.y);
                Console.WriteLine("z " + multiplicationCoord(stPoint.x, stPoint.y));
                Console.WriteLine("sideLength " + sl / this.deltaSideLength);
                ++iteration;
                sl = sl / this.deltaSideLength;
            } while (iteration < this.iterationCount /*&& sl >= minSideLength*/);

            return stPoint;
        }

        private float multiplicationCoord(float x, float y)
        {
            return (float)(100 * Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
            //return (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
        }
    }
}
