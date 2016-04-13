using System;
using System.Linq;

namespace OPR.lb1
{
    public class HyperCube
    {
        private readonly SquarePoint startPoint;
        private readonly float sideLength, iterationCount, minSideLength, deltaSideLength;
        private readonly int innerPointsCount;
        private readonly bool isDebugMode;

        public HyperCube(
            SquarePoint startpoint,
            float sideLength,
            float minSideLength,
            float deltaSideLenth,
            int iterationCount,
            int innerPointsCount,
            bool isDebugMode = false)
        {
            this.deltaSideLength = deltaSideLenth;
            this.startPoint = startpoint;
            this.sideLength = sideLength;
            this.iterationCount = iterationCount;
            this.minSideLength = minSideLength;
            this.innerPointsCount = innerPointsCount;
            this.isDebugMode = isDebugMode;
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
                    .Select(x => new { point = x, result = MultiplicationCoord(x.x, x.y) })
                    .OrderBy(x => x.result).ToList();
                stPoint = stPoints
                    .First().point;

                if (isDebugMode)
                {
                    Console.WriteLine("\nBest point on {0} iteration :", iteration);
                    Console.WriteLine("x " + stPoint.x);
                    Console.WriteLine("y " + stPoint.y);
                    Console.WriteLine("z " + MultiplicationCoord(stPoint.x, stPoint.y));
                    Console.WriteLine("sideLength " + sl / this.deltaSideLength);
                }

                ++iteration;
                sl = sl / this.deltaSideLength;
            } while (iteration < this.iterationCount /*&& sl >= minSideLength*/);

            return stPoint;
        }

        protected virtual float MultiplicationCoord(float x, float y)
        {
            return (float)(100 * Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
            //return (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
        }
    }
}
