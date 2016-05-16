using OPR.lb2.Interfaces.Common;

namespace OPR.SSGA2.Italik
{
    public sealed class BinarySSGA : SSGA2<BinaryValueService, BinaryGenom>
    {
        public BinarySSGA(
            ISeparator<Entity<BinaryValueService, BinaryGenom>> firstStepSeprator,
            ISeparator<Entity<BinaryValueService, BinaryGenom>> commonSeparator,
            IArgsGenerator generator) : base(firstStepSeprator, commonSeparator, generator)
        {
        }
    }
}
