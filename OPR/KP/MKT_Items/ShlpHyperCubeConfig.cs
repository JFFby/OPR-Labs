using System;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb1;

namespace OPR.KP.MKT_Items
{
    public sealed class ShlpHyperCubeConfig
    {
        public float SideLength { get; set; }

        public float DeltaSideLenth { get; set; }

        public int IterationCount { get; set; }

        public int InnerPointsCount { get; set; }

        public Func<float, float, float> Fn { get; set; }

        public SquarePoint[] Bounds { get; set; }

        public MktIterationMode IterationMode { get; set; }
    }
}
