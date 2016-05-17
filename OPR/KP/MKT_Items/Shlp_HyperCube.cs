using System;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb1;

namespace OPR.KP.MKT_Items
{
    public sealed class ShlpHyperCube : HyperCube
    {
        public readonly Func<float, float, float> fn;

        public ShlpHyperCube(
            SquarePoint startpoint,
            float sideLength,
            float deltaSideLenth,
            int iterationCount,
            int innerPointsCount,
            SquarePoint[] bounds,
            Func<float, float, float> fn,
            MktIterationMode iterationMode) : base(startpoint,
                sideLength,
                deltaSideLenth,
                iterationCount,
                innerPointsCount,
                bounds,
                iterationMode,
                false)
        {
            this.fn = fn;
        }

        protected override float MultiplicationCoord(float x, float y)
        {
            return fn(x, y);
        }
    }
}
