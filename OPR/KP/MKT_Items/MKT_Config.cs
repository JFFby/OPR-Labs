using OPR.KP.Shlp;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;

namespace OPR.KP.MKT_Items
{
    public sealed class MKT_Config : EntityArgs
    {
        public IShlpWrapper Shlp {get; set; }

        public IGenerator<MKT_Point> Generator { get; set; }

        public ISeparator<MKT_Point> Separator { get; set; }
        
        public int N { get; set; }
         
        public int n { get; set; } 
        
        public byte Lambda { get; set; }

        public MktIterationMode IterationMode { get; set; }

        public override string ToString()
        {
            return string.Join("_",
                GetBoundString(GlobalSettings.LeftXBound, GlobalSettings.RightXBound),
                GetBoundString(GlobalSettings.BottomYBound, GlobalSettings.TopYBound),
                Shlp.ShlpType, Generator.GetType().Name, Separator.GetType().Name, N, n, Lambda);
        }

        public string GetBoundString(float from, float to)
        {
            return string.Format("{0}-{1}",from, to);
        }
    }
}
