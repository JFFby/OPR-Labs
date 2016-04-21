using System;
using OPR.KP.MKT_Items;
using OPR.lb1;

namespace OPR.KP.Shlp
{
    public sealed class HyperCubeWrapper : IShlpWrapper
    {
        private readonly ShlpHyperCubeConfig config;

        public HyperCubeWrapper(Func<float,float,float> fn, SquarePoint[] bounds)
        {
            this.config =  new ShlpHyperCubeConfig
            {
                Fn = fn,
                DeltaSideLenth = 1.1f,
                InnerPointsCount = 5,
                SideLength = 1,
                IterationCount = 15,
                Bounds = bounds
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
              config.Fn);
        }

        public ShlpType ShlpType { get {return ShlpType.HyperCube;} }
    }
}
