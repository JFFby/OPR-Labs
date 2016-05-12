using System.Collections.Generic;
using System.Linq;
using System;
using OPR.lb2.Enums;

namespace OPR.lb2
{
    public sealed class BinaryGeneration : Generation<BinaryGenom>
    {
        private BinaryEntity worstEntity;
        private IList<BinaryEntity> binaryGeneration;

        public BinaryGeneration(List<Entity<BinaryGenom>> entities) : base(entities)
        {
            binaryGeneration = genetation.Cast<BinaryEntity>().ToList();
            for (int i = 0; i < binaryGeneration.Count; i++)
            {
                binaryGeneration[i].Function = EntityFunction.None;
                binaryGeneration[i].Id = i + 1;
            }
        }

        public IList<BinaryEntity> Parents
        {
            get { return binaryGeneration; }
        }

        public IList<BinaryEntity> Entities
        {
            get { return binaryGeneration.Union(
                Children?.OfType<BinaryEntity>() ?? new List<BinaryEntity>()).ToList(); }
            set { binaryGeneration = value; }
        }

        public void AddChilds(IList<BinaryEntity> childs)
        {
            Children = Children ?? new List<Entity<BinaryGenom>>();
            for (int i = 0; i < childs.Count; i++)
            {
                var child = childs[i] as BinaryEntity;
                if (child != null)
                {
                    child.Id = genetation.Count + Children.Count + 1;
                    Children.Add(child);
                }
            }
        }

        public IList<BinaryEntity> Winners()
        {
            return binaryGeneration.Where(x => !x.Equals(worstEntity)).ToList();
        }

        public override IList<Entity<BinaryGenom>> GetBest(int take)
        {
            var best = binaryGeneration.OrderBy(x => x.Value).Take(take).ToList();
            best.ForEach(x => x.Function = EntityFunction.BestParent);
            return best.Cast<Entity<BinaryGenom>>().ToList();
        }

        public IList<Entity<BinaryGenom>> ClearValueOfParent()
        {
            var all = binaryGeneration.ToList();
            all.ForEach(x => x.Function = EntityFunction.None);
            return all.Cast<Entity<BinaryGenom>>().ToList();
        }

        public override Entity<BinaryGenom> GetWorst()
        {
            worstEntity = binaryGeneration.OrderByDescending(x => x.Value).First();
            worstEntity.Function = EntityFunction.WorstParent;
            return worstEntity;
        }

        public int GrossPoint
        {
            get
            {
                var childenity =
                    this.Children.OfType<BinaryEntity>().FirstOrDefault(x => x.Genom.Type == EntityType.Child);
                return childenity != null
                    ? childenity.Genom.CrossingPoint.GetValueOrDefault()
                    : 0;
            }
        }

        public BinaryGeneration MarkUpGenereation()
        {
            GetWorst();
            GetBest(2);
            return this;
        }
    }
}
