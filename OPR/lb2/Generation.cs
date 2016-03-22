using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Interfaces;

namespace OPR.lb2
{
    public abstract class Generation<T> where T:IGenom, new ()
    {
        private static int identity = 0;
        protected readonly List<Entity<T>>  genetation = new List<Entity<T>>();

        protected Generation(List<Entity<T>> entities)
        {
            genetation = entities;
            Id = ++identity;
        }

        public int Id { get; set; }

        public IList<Entity<T>> Children { get; set; }

        public abstract IList<Entity<T>> GetBest(int take);

        public abstract Entity<T> GetWorst();
    }
}