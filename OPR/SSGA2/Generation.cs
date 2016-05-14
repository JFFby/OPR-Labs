using System.Collections.Generic;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public sealed class Generation<TValueService, TGenom> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {
        private List<Entity<TValueService, TGenom>> generation = new List<Entity<TValueService, TGenom>>();
    }
}
