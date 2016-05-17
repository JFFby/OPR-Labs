using System;
using OPR.KP.MKT_Items;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb1;

namespace OPR.KP.Shlp
{
    public sealed class HyperCubeWrapper : IShlpWrapper
    {
        private readonly ShlpHyperCubeConfig config;

        public HyperCubeWrapper(Func<float,float,float> fn, SquarePoint[] bounds, MktIterationMode iterationMode)
        {
            this.config =  new ShlpHyperCubeConfig
            {
                Fn = fn,
                DeltaSideLenth = 1.1f,
                InnerPointsCount = 5,
                SideLength = 1,
                IterationCount = 15,
                Bounds = bounds,
                IterationMode = iterationMode
            };
        }

        public IShlp GetShlpObject(SquarePoint point)
        {
           return new ShlpHyperCube(
              point,
              config.SideLength,
              config.DeltaSideLenth,
              config.IterationCount,
              config.InnerPointsCount,
              config.Bounds,
              config.Fn,
              config.IterationMode);
        }

        public ShlpType ShlpType { get {return ShlpType.HyperCube;} }
    }
}
