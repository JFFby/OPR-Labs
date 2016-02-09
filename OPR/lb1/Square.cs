using System;
using System.Collections.Generic;
using System.Linq;

namespace OPR.lb1
{
    public sealed class Square
    {
        private SquarePoint startPoint;
        private float sideLength;

        private Square(SquarePoint startPoint, float sideLength)
        {
            this.startPoint = startPoint;
            this.sideLength = sideLength;
        }

        public IList<SquarePoint> BoundPoints { get; private set; }

        public IList<SquarePoint> InnerPoints { get; private set; }

        public static Square GeneratePointsFromLeftCorner(SquarePoint startPoint, float sideLength, int innerpointsCount)
        {
            startPoint.Number = 1;
            var points = new List<SquarePoint> { startPoint };
            points.Add(new SquarePoint(startPoint.x, startPoint.y + sideLength, 2));
            points.Add(new SquarePoint(startPoint.x + sideLength, startPoint.y + sideLength, 3));
            points.Add(new SquarePoint(startPoint.x + sideLength, startPoint.y, 4));
            var square = new Square(startPoint, sideLength) { BoundPoints = points };
            square.CreateInnerPoints(innerpointsCount);
            return square;
        }

        public static Square GeneratePointsFromCenter(SquarePoint startPoint, float sideLength, int innerpointsCount)
        {
            startPoint.Number = 1;
            var points = new List<SquarePoint> { startPoint };
            /// calculating other points
            throw  new NotImplementedException();
            var square = new Square(startPoint, sideLength) { BoundPoints = points };
            square.CreateInnerPoints(innerpointsCount);
            return square;
        }

        public void CreateInnerPoints(int innerpointsCount)
        {
            var points = new List<SquarePoint>();
            var point3 = this.BoundPoints.Single(x => x.Number == 3);
            var maxX = point3.x * 100;
            var maxY = point3.y * 100;
            var point1 = this.BoundPoints.Single(x => x.Number == 1);
            var minX = point1.x * 100;
            var minY = point1.y * 100;
            var rnd = new Random(DateTime.Now.GetHashCode());
            for (int i = 0; i < innerpointsCount; i++)
            {
                var rndX = minX + rnd.NextDouble() * (maxX - minX);
                var rndY = minY + rnd.NextDouble() * (maxY - minY);
                points.Add(new SquarePoint((float)rndX, (float)rndY));
            }

            InnerPoints = points;
        }
    }
}
