using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2.Italik
{
    public sealed class BinarySSGA : SSGA2<BinaryValueService, BinaryGenom>
    {
        public BinarySSGA(
            ISeparator<IValue> firstStepSeprator,
            ISeparator<Entity<BinaryValueService, BinaryGenom>> commonSeparator,
            IArgsGenerator generator) : base(firstStepSeprator, commonSeparator, generator)
        {
        }
    }
}
