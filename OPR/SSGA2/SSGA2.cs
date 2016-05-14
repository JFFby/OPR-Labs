
using System.Collections.Generic;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public sealed class SSGA2<TValueService, TGenom> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {
        private readonly ISeparator<Entity<TValueService, TGenom>> Separator;
        private readonly IGenerator<Entity<TValueService, TGenom>> Generator;

        public SSGA2(ISeparator<Entity<TValueService, TGenom>> separator,
            IGenerator<Entity<TValueService, TGenom>> generator)
        {
            Separator = separator;
            Generator = generator;
        }

        //public List<Generation<TValueService, TGenom>> Start()
        //{
            
        //}
    }
}
