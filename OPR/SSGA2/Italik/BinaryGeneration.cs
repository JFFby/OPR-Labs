using System.Collections.Generic;

namespace OPR.SSGA2.Italik
{
    public sealed class BinaryGeneration:Generation<BinaryValueService, BinaryGenom>
    {
        public BinaryGeneration(List<Entity<BinaryValueService, BinaryGenom>> entities) : base(entities)
        {
        }
    }
}
