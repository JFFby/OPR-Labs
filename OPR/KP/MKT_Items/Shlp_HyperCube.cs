using System;
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
            bool isDebugMode = false) : base(startpoint,
                sideLength,
                deltaSideLenth,
                iterationCount,
                innerPointsCount,
                bounds,
                isDebugMode)
        {
            this.fn = fn;
        }

        protected override float MultiplicationCoord(float x, float y)
        {
            return fn(x, y);
        }
    }
}
