using System.Collections.Generic;
using OPR.SSGA2;

namespace OPR.KP.SSGA_MKT_Items
{
    public sealed class MktGeneration : Generation<MktValueService, MktGenom>
    {
        public MktGeneration(List<Entity<MktValueService, MktGenom>> entities, bool isIncreaseId = true) : base(entities, isIncreaseId)
        {
        }
    }
}
