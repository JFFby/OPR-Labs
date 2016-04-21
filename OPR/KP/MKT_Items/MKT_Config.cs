using System;
using OPR.KP.Shlp;
using OPR.lb1;
using OPR.lb2.Interfaces.Common;

namespace OPR.KP.MKT_Items
{
    public sealed class MKT_Config
    {
        public IShlpWrapper Shlp {get; set; }

        public IGenerator<MKT_Point> Generator { get; set; }

        public ISeparator<MKT_Point> Separator { get; set; }
        
        public int N { get; set; }
         
        public int n { get; set; } 

        public int Iterations { get; set; } 

        public SquarePoint [] Bounds { get; set; }

        public byte Lambda { get; set; }

        public Func<float, float, float> Fn { get; set; }

        public override string ToString()
        {
            return string.Join("_", GetBoundString(Bounds[0].x, Bounds[1].x), GetBoundString(Bounds[0].y, Bounds[1].y)
                , Shlp.ShlpType, Generator.GetType().Name, Separator.GetType().Name, N, n, Iterations, Lambda);
        }

        public string GetBoundString(float from, float to)
        {
            return string.Format("{0}-{1}",from, to);
        }
    }
}
