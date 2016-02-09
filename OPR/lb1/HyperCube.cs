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
            do
            {
                var square = iteration == 0
                    ? Square.GeneratePointsFromLeftCorner(stPoint, sideLength, innerPointsCount)
                    : Square.GeneratePointsFromCenter(stPoint, sideLength, innerPointsCount);
                // test this query
                // test this algoritm :)
                stPoint = square.InnerPoints
                    .Select(x => new { point = x, fn = Function(x.x, x.y) })
                    .OrderBy(x => x.fn)
                    .First().point;
                ++iteration;
            } while (iteration < this.iterationCount);

            return stPoint;
        }

        private float Function(float x, float y)
        {
            return x + y;
        }
    }
}
