using System.Collections.Generic;
using System.Linq;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2.Extension
{
    public static class GenerationExtension
    {
        public static List<Entity<TValueService, TGenom>> CreateNewEntities<TValueService, TGenom>(this List<Entity<TValueService, TGenom>> bestEntities)
            where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            if (bestEntities.Count != 2)
            {
                throw new KeyNotFoundException();
            }

            var first = bestEntities.First();
            var second = bestEntities[1];
            return first.CreateChildEntity(second);
        }

        public static Generation<TValueService, TGenom> ToGeneration<TValueService, TGenom>(
            this IEnumerable<Entity<TValueService, TGenom>> entities)
            where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            return new SSGA2.Generation<TValueService, TGenom>(entities.ToList());
        }

        public static int GetChilrensCrossingPoint<TValueService, TGenom>(this Generation<TValueService, TGenom> generation)
           where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            var firstchild = generation.Children.FirstOrDefault();
            return firstchild != null && firstchild.CrossPoint.HasValue ? -firstchild.CrossPoint.Value : -1;
        }
    }
}
