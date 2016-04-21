using System;
using OPR.KP.MKT_Items;
using OPR.lb1;

namespace OPR.KP.Shlp.NelderMid
{
    public sealed class NelderMidWrapper : IShlpWrapper
    {
        private readonly Func<float, float, float> fn;
        private readonly SquarePoint[] bounds;

        public NelderMidWrapper(Func<float, float, float> fn, SquarePoint[] bounds)
        {
            this.fn = fn;
            this.bounds = bounds;
        }

        public IShlp GetShlpObject(SquarePoint point)
        {
            return new NelderMid((MKT_Point)point, fn, bounds);
        }

        public ShlpType ShlpType { get { return ShlpType.NelderMid; } }
    }
}
