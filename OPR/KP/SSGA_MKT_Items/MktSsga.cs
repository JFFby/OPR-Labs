using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;

namespace OPR.KP.SSGA_MKT_Items
{
    public sealed class MktSsga : SSGA2<MktValueService, MktGenom>
    {
        public MktSsga(
            ISeparator<Entity<MktValueService, MktGenom>> firstStepSeprator,
            ISeparator<Entity<MktValueService, MktGenom>> commonSeparator,
            IArgsGenerator generator) : base(firstStepSeprator, commonSeparator, generator)
        {
        }
    }
}
