using System;
using System.Linq;
using OPR.KP.Shlp;
using OPR.KP.SSGA_MKT_Items;

namespace OPR.lb1
{
    public class HyperCube : IShlp
    {
        protected readonly SquarePoint startPoint;
        private readonly float sideLength, iterationCount, deltaSideLength;
        private readonly int innerPointsCount;
        private readonly bool isDebugMode;
        private readonly SquarePoint[] bounds;

        public HyperCube(
            SquarePoint startpoint,
            float sideLength,
            float deltaSideLenth,
            int iterationCount,
            int innerPointsCount,
            SquarePoint[] bounds,
            MktIterationMode iterationMode,
            bool isDebugMode = false)
        {
            this.deltaSideLength = deltaSideLenth;
            this.startPoint = startpoint;
            this.sideLength = sideLength;
            this.iterationCount = iterationMode == MktIterationMode.Full ? iterationCount : 2;
            this.innerPointsCount = innerPointsCount;
            this.isDebugMode = isDebugMode;
            this.bounds = bounds;
        }

        public SquarePoint Calculate()
        {

            SquarePoint stPoint = startPoint;
            int iteration = 0;
            float sl = this.sideLength;
            do
            {
                var square = iteration == 0
                    ? Square.GeneratePointsFromLeftCorner(stPoint, sl, innerPointsCount, bounds)
                    : Square.GeneratePointsFromCenter(stPoint, sl, innerPointsCount, bounds);

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
            } while (iteration < this.iterationCount);

            return stPoint;
        }

        protected virtual float MultiplicationCoord(float x, float y)
        {
            return (float)(100 * Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
        }
    }
}
