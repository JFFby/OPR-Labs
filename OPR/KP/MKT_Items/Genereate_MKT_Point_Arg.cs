using System;
using OPR.lb1;

namespace OPR.KP.MKT_Items
{
    public sealed class Genereate_MKT_Point_Arg
    {
        public SquarePoint [] Bounds { get; set; }

        public Func<float, float, float> fn { get; set; } 
    }
}
