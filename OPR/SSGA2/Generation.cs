using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Enums;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public sealed class Generation<TValueService, TGenom> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {
        private static int identity = 0;

        private Entity<TValueService, TGenom> worstEntity;

        private List<Entity<TValueService, TGenom>> generation = new List<Entity<TValueService, TGenom>>();

        public Generation(List<Entity<TValueService, TGenom>> entities)
        {
            generation = entities;
            Id = ++identity;
            
            for (int i = 0; i < generation.Count; i++)
            {
                generation[i].Function = EntityFunction.None;
                generation[i].Id = i + 1;
            }
        }

        public int Id { get; set; }

        public List<Entity<TValueService, TGenom>> Entites { get { return generation; } }

        public List<Entity<TValueService, TGenom>> Winners()
        {
            return generation.Where(x => !x.Equals(worstEntity))
                .ToList();
        }

        public Generation<TValueService, TGenom> MarkUpGenereation(ISeparator<Entity<TValueService, TGenom>> separator)
        {
            GetBest(separator);
            GetWorst(separator);
            return this;
        }

        private void GetWorst(ISeparator<Entity<TValueService, TGenom>> separator)
        {
            worstEntity = separator.Separate(generation, 1, false).First();
            worstEntity.Function = EntityFunction.WorstParent;
        }

        private void GetBest(ISeparator<Entity<TValueService, TGenom>> separator)
        {
            var best = separator.Separate(generation, 2, true).ToList();
            best.ForEach(x => x.Function = EntityFunction.BestParent);
        }
    }
}
