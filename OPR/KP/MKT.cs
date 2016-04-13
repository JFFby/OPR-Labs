using OPR.lb1;
using OPR.lb2.Interfaces.Common;

namespace OPR.KP
{
    public sealed class MKT
    {
        private readonly HyperCube hyperCube;
        private readonly ISeparator separator;
        private readonly IGenerator generator;

        public MKT(
            HyperCube hyperCube,
            ISeparator separator,
            IGenerator generator)
        {
            this.hyperCube = hyperCube;
            this.separator = separator;
            this.generator = generator;
        }
    }
}
