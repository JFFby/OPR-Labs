using System;
using System.Collections.Generic;
using OPR.lb1;
using OPR.lb2;
using OPR.lb2.Interfaces.Common;

namespace OPR.KP.MKT_Items
{
    public sealed class RandomGenerator : IGenerator<SquarePoint>
    {
        public IList<SquarePoint> Generate(int count, object state)
        {
            var bounds = GetBounds(state);
            var points = new List<SquarePoint>();
            for (int i = 0; i < count; ++i)
            {
                points.Add(new SquarePoint(
                    RandomHelper.RandomFloat(bounds[0].x, bounds[1].x),
                     RandomHelper.RandomFloat(bounds[0].y, bounds[1].y)));
            }

            return points;
        }

        private SquarePoint[] GetBounds(object state)
        {
            var bounds = state as SquarePoint [];
            if (bounds == null || bounds.Length != 2)
            {
                throw new ArgumentException("state");
            }

            return bounds;
        }
    }
}
