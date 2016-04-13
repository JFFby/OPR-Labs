using System;
using OPR.lb1;

namespace OPR.KP.MKT_Items
{
    public sealed class ShlpHyperCube : HyperCube
    {
        private readonly Func<float, float, float> fn;

        public ShlpHyperCube(
            SquarePoint startpoint,
            float sideLength,
            float minSideLength,
            float deltaSideLenth,
            int iterationCount,
            int innerPointsCount,
            Func<float, float, float> fn,
            bool isDebugMode = false) : base(startpoint,
                sideLength,
                minSideLength,
                deltaSideLenth,
                iterationCount,
                innerPointsCount,
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
