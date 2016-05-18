using System;
using System.Collections.Generic;
using System.Linq;
using OPR.KP.MKT_Items;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb1;

namespace OPR.KP.Shlp.NelderMid
{
    /// <summary>
    /// https://ru.wikipedia.org/wiki/%D0%9C%D0%B5%D1%82%D0%BE%D0%B4_%D0%9D%D0%B5%D0%BB%D0%B4%D0%B5%D1%80%D0%B0_%E2%80%94_%D0%9C%D0%B8%D0%B4%D0%B0
    /// </summary>
    public sealed class NelderMid : IShlp
    {
        private readonly float alpha = 1, beta = 0.5f, gamma = 2, initialLength = 0.1f, minLength = 0.01f;
        private readonly MKT_Point startPoint;
        private readonly Func<float, float, float> fn;
        private readonly IList<MKT_Point> _points;
        private readonly SquarePoint[] bounds;
        private MKT_Point[] currentPoints;
        private readonly int hi = 0, gi = 1, li = 2, maxPoints;

        public NelderMid(
            MKT_Point startPoint,
            Func<float, float, float> fn,
            SquarePoint[] bounds,
            MktIterationMode iterationMode)
        {
            this.startPoint = startPoint;
            this.fn = fn;
            _points = new List<MKT_Point>();
            this.bounds = bounds;
            currentPoints = new MKT_Point[3];
            maxPoints = iterationMode == MktIterationMode.Full ? 75 : 6;
        }

        public SquarePoint Calculate()
        {
            GetStartPoints();
            return Iteration();
        }

        private SquarePoint Iteration()
        {
            do
            {
                var isComressionNeeded = false;
                currentPoints = currentPoints.OrderByDescending(x => x.Value).ToArray();
                var xh = currentPoints[hi];
                var xg = currentPoints[gi];
                var xl = currentPoints[li];

                if (float.IsInfinity(xl.Value))
                {
                    break;
                }

                var xc = new SquarePoint(
                    (xg.x + xl.x) / 2,
                    (xg.y + xl.y) / 2);
                var xr = new MKT_Point(
                    Xr(xc.x, xh.x),
                    Xr(xc.y, xh.y),
                    fn);

                if (xr.Value < xl.Value)
                {
                    var xe = new MKT_Point(
                        Xe(xc.x, xr.x),
                        Xe(xc.y, xr.y),
                        fn);
                    if (xe.Value <= xr.Value)
                    {
                        SetupCurrentPoint(xe, hi);
                    }
                    else
                    {
                        SetupCurrentPoint(xr, hi);
                    }
                }
                else if (xl.Value < xr.Value && xr.Value < xh.Value)
                {
                    SetupCurrentPoint(xr, hi);
                }
                else if (xg.Value < xr.Value && xr.Value < xh.Value)
                {
                    var buf_xr = xr;
                    xr = xh;
                    xh = buf_xr;
                    isComressionNeeded = true;
                }
                else
                {
                    isComressionNeeded = true;
                }

                if (isComressionNeeded)
                {
                    var xs = new MKT_Point(Xs(xh.x, xc.x), Xs(xh.y, xc.y), fn);
                    if (xs.Value < xh.Value)
                    {
                        SetupCurrentPoint(xs, hi);
                    }
                    else
                    {
                        var xi_xh = new MKT_Point(Xi(xh.x, xl.x), Xi(xh.y, xl.y), fn);
                        SetupCurrentPoint(xi_xh, hi);
                        var xi_xg = new MKT_Point(Xi(xg.x, xl.x), Xi(xg.y, xl.y), fn);
                        SetupCurrentPoint(xi_xg, hi);
                    }
                }

            } while (OneMoreIteration());

            var suitablePoints =
                _points.Where(x => x.x <= bounds[1].x && x.x >= bounds[0].x && x.y >= bounds[0].y && x.y <= bounds[1].y);
            return suitablePoints
                .FirstOrDefault(x => Math.Abs(x.Value - suitablePoints.Min(c => c.Value)) < 0.01);
        }

        private bool OneMoreIteration()
        {
            var values = new List<float>();
            for (int i = 0; i < currentPoints.Length; i++)
            {
                for (int j = 1; j < currentPoints.Length; j++)
                {
                    if (currentPoints[i].Equals(currentPoints[j])) continue;

                    values.Add(Length(currentPoints[i], currentPoints[j]));
                }
            }

            return values.Max() > minLength && _points.Count < maxPoints;
        }

        private float Length(MKT_Point p1, MKT_Point p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
        }

        private float Xi(float xi, float xl)
        {
            return xl + (xi - xl) / 2;
        }

        private float Xs(float xh, float xc)
        {
            return beta * xh + (1 - beta) * xc;
        }

        private float Xe(float xc, float xr)
        {
            return (1 - gamma) * xc + gamma * xr;
        }

        private float Xr(float xc, float xh)
        {
            return (1 + alpha) * xc - alpha * xh;
        }

        private MKT_Point[] GetStartPoints()
        {
            SetupCurrentPoint(startPoint, 0);
            var secondPoint = new MKT_Point(startPoint.x + initialLength, startPoint.y, fn);
            var thirdPoint = new MKT_Point(startPoint.x, startPoint.y + initialLength, fn);
            SetupCurrentPoint(secondPoint, 1);
            SetupCurrentPoint(thirdPoint, 2);
            return currentPoints;
        }

        private void SetupCurrentPoint(MKT_Point point, int index)
        {
            _points.Add(point);
            currentPoints[index] = point;
        }
    }
}
