using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb2;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using OPR.SSGA2.Interfaces;

namespace OPR.KP.MKT_Items
{
    public sealed class RandomGenerator : IGenerator<MKT_Point>
    {
        public readonly IConverter converter;
        public object state;

        public RandomGenerator(IConverter converter)
        {
            this.converter = converter;
        }

        public IList<MKT_Point> Generate(int count)
        {
            var bounds = GetBounds();
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

        public IList<EntityArgs> GenerateEntityArgs(int count)
        {
            return Generate(count).Select(converter.Convert).ToList();
        }

        public void SetupState(dynamic state)
        {
            var property = state.GetType().GetProperty("state");
            this.state = property.GetValue(state);
        }

        private Genereate_MKT_Point_Arg GetBounds()
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
