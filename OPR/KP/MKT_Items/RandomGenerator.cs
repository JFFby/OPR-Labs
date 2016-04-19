using System;
using System.Collections.Generic;
using OPR.lb2;
using OPR.lb2.Interfaces.Common;

namespace OPR.KP.MKT_Items
{
    public sealed class RandomGenerator : IGenerator<MKT_Point>
    {
        public IList<MKT_Point> Generate(int count, object state)
        {
            var bounds = GetBounds(state);
            var points = new List<MKT_Point>();
            for (int i = 0; i < count; ++i)
            {
                points.Add(new MKT_Point(
                    RandomHelper.RandomFloat(bounds.Bounds[0].x, bounds.Bounds[1].x),
                     RandomHelper.RandomFloat(bounds.Bounds[0].y, bounds.Bounds[1].y),
                     bounds.fn));
            }

            return points;
        }

        private Genereate_MKT_Point_Arg GetBounds(object state)
        {
            var bounds = state as Genereate_MKT_Point_Arg;
            if (bounds == null || bounds.Bounds.Length != 2)
            {
                throw new ArgumentException("state");
            }

            return bounds;
        }
    }
}
