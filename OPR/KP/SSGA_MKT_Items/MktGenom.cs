using System.Collections.Generic;
using OPR.SSGA2;
using IGenom = OPR.SSGA2.Interfaces.IGenom;

namespace OPR.KP.SSGA_MKT_Items
{
    public sealed class MktGenom : Genom, IGenom
    {
        public MktGenom()
        {
            cromosomeCreator = new MktChromosome();
        }

        public string Code { get; set; }

        public void Initializae(EntityArgs args)
        {
            _code = cromosomeCreator.EntityArgsToCode(args);
            Code = string.Join("-", _code);
        }
    }
}
