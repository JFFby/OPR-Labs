using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Enums;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public class Generation<TValueService, TGenom> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {
        private static int identity = 0;

        private Entity<TValueService, TGenom> worstEntity;

        private List<Entity<TValueService, TGenom>> generation = new List<Entity<TValueService, TGenom>>();
        private List<Entity<TValueService, TGenom>> children = new List<Entity<TValueService, TGenom>>();
        private ISeparator<Entity<TValueService, TGenom>> chachedSeparator; // not good
        
        public Generation(List<Entity<TValueService, TGenom>> entities, bool isIncreaseId = true)
        {
            generation = entities;

            if (isIncreaseId)
            {
                Id = ++ identity;
                for (int i = 0, n = 0; i < generation.Count; i++)
                {
                    generation[i].Function = EntityFunction.None;
                    if (generation[i].Id == 0)
                    {
                        generation[i].Id = ++n;
                    }

                    if (generation[i].GenerationId == 0)
                    {
                        generation[i].GenerationId = Id;
                    }
                }
            }
        }

        public List<Entity<TValueService, TGenom>> Parents
        {
            get { return generation.Where(x => x.Type == EntityType.Parent).ToList(); }
        }

        public List<Entity<TValueService, TGenom>> Children
        {
            get { return children; }
        }

        public int Id { get; set; }

        public List<Entity<TValueService, TGenom>> Entites { get { return generation; } }

        public void AddChildern(IList<Entity<TValueService, TGenom>> children)
        {
            this.children.AddRange(children);
            MarkUpChildern();
        }

        public List<Entity<TValueService, TGenom>> Winners()
        {
            return generation.Where(x => !x.Equals(worstEntity))
                .ToList();
        }

        public Generation<TValueService, TGenom> MarkUpGenereation(ISeparator<Entity<TValueService, TGenom>> separator)
        {
            chachedSeparator = separator;
            GetBest(separator);
            GetWorst(separator);
            return this;
        }

        public Generation<TValueService, TGenom> MarkAllEntitrsAsParents()
        {
            foreach (var entity in generation)
            {
                entity.MargAsParent();
            }

            return this;
        }

        private void GetWorst(ISeparator<Entity<TValueService, TGenom>> separator)
        {
            worstEntity = separator.Separate(generation, 1, false).First();
            worstEntity.Function = EntityFunction.WorstParent;
        }

        private void GetBest(ISeparator<Entity<TValueService, TGenom>> separator)
        {
            var bestParents = separator.Separate(Parents, 2, true).ToList();
            bestParents.ForEach(x => x.Function = EntityFunction.BestParent);
            MarkUpChildern();
        }

        private void MarkUpChildern()
        {
            if (children.Any())
            {
                var bestChildren = chachedSeparator.Separate(children, 1, true).ToList();
                bestChildren.ForEach(x => x.Function = EntityFunction.BestChild);

                for (int i = 0; i < children.Count; i++)
                {
                    children[i].GenerationId = Id + 1;
                    children[i].Id = i + 1;
                }
            }
        }
    }
}
