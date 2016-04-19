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

      /*  private void log(IList<SquarePoint> points )
        {
            Console.WriteLine("\nSquare :");
            foreach (SquarePoint value in points)
            {
                Console.WriteLine("Number " + value.Number);
                Console.WriteLine("x " + value.x);
                Console.WriteLine("y " + value.y);
            }
            Console.WriteLine("---------");
        }
       */        
        public IList<SquarePoint> BoundPoints { get; private set; }

        public IList<SquarePoint> InnerPoints { get; private set; }

        public static Square GeneratePointsFromLeftCorner(SquarePoint startPoint, float sideLength, int innerpointsCount, SquarePoint[] bounds)
        {
            startPoint.Number = 1;
            var points = new List<SquarePoint> { startPoint };
            points.Add(new SquarePoint(startPoint.x, startPoint.y + sideLength, 2));
            points.Add(new SquarePoint(startPoint.x + sideLength, startPoint.y + sideLength, 3));
            points.Add(new SquarePoint(startPoint.x + sideLength, startPoint.y, 4));
            var square = new Square(startPoint, sideLength) { BoundPoints = points };
           /* square.log(square.BoundPoints);*/
            square.CreateInnerPoints(innerpointsCount, bounds);

            return square;
        }

        public static Square GeneratePointsFromCenter(SquarePoint startPoint, float sideLength, int innerpointsCount, SquarePoint[] bounds)
        {
            startPoint.Number = 1;
            var points = new List<SquarePoint>();
            points.Add(new SquarePoint(startPoint.x - sideLength, startPoint.y - sideLength, 1));
            points.Add(new SquarePoint(startPoint.x - sideLength, startPoint.y + sideLength, 2));
            points.Add(new SquarePoint(startPoint.x + sideLength, startPoint.y + sideLength, 3));
            points.Add(new SquarePoint(startPoint.x + sideLength, startPoint.y - sideLength, 4));

            var square = new Square(startPoint, sideLength) { BoundPoints = points };
            /*square.log(square.BoundPoints);*/
            square.CreateInnerPoints(innerpointsCount, bounds);
            return square;
        }

        public void CreateInnerPoints(int innerpointsCount, SquarePoint[] bounds)
        {
            var s  = this.BoundPoints;
            var points = new List<SquarePoint>();
            var point3 = this.BoundPoints.Single(x => x.Number == 3);
            var maxX = Math.Min(point3.x, bounds[1].x);
            var maxY = Math.Min(point3.y, bounds[1].y);
            var point1 = this.BoundPoints.Single(x => x.Number == 1);
            var minX = Math.Max(point1.x, bounds[0].x);
            var minY = Math.Max(point1.y, bounds[0].y);
            var rnd = new Random(DateTime.Now.GetHashCode());
            for (int i = 0; i < innerpointsCount; i++)
            {
                var rndX = minX + rnd.NextDouble() * (maxX - minX);
                var rndY = minY + rnd.NextDouble() * (maxY - minY);
                points.Add(new SquarePoint((float)rndX, (float)rndY, i));
            }
            InnerPoints = points;
        }
    }
}
