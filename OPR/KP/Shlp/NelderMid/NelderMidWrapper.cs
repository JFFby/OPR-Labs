using System;
using OPR.KP.MKT_Items;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb1;

namespace OPR.KP.Shlp.NelderMid
{
    public sealed class NelderMidWrapper : IShlpWrapper
    {
        private readonly Func<float, float, float> fn;
        private readonly SquarePoint[] bounds;
        private readonly MktIterationMode iterationMode;

        public NelderMidWrapper(Func<float, float, float> fn, SquarePoint[] bounds, MktIterationMode iterationMode)
        {
            this.fn = fn;
            this.bounds = bounds;
            this.iterationMode = iterationMode;
        }

        public IShlp GetShlpObject(SquarePoint point)
        {
            return new NelderMid((MKT_Point)point, fn, bounds, iterationMode);
        }

        public ShlpType ShlpType { get { return ShlpType.NelderMid; } }
    }
}
