using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2.Extension
{
    public static class GenerationExtension
    {
        public static List<Entity<TValueService, TGenom>> CreateNewEntities<TValueService, TGenom>(this List<Entity<TValueService, TGenom>> bestEntities)
            where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            var first = bestEntities.First();
            var second = bestEntities[1];
            return first.CreateChildEntity(second);
        }

        public static Generation<TValueService, TGenom> ToGeneration<TValueService, TGenom>(
            this IEnumerable<Entity<TValueService, TGenom>> entities)
            where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            return new Generation<TValueService, TGenom>(entities.ToList());
        }

        [Obsolete("Do Not use it anywhere!")]
        public static Generation<TValueService, TGenom> ToMarkedGeneration<TValueService, TGenom>(
           this IEnumerable<Entity<TValueService, TGenom>> entities,
           ISeparator<Entity<TValueService, TGenom>> separator)
           where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            return new Generation<TValueService, TGenom>(entities.ToList(), false).MarkUpGenereation(separator);
        }

        public static string GetChilrensCrossingPoint<TValueService, TGenom>(this Generation<TValueService, TGenom> generation)
           where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            var firstchild = generation.Children.FirstOrDefault();
            return firstchild != null && !string.IsNullOrEmpty(firstchild.CrossPoint) ? firstchild.CrossPoint : string.Empty;
        }

        public static List<Entity<TValueService, TGenom>> SetIds<TValueService, TGenom>(this List<Entity<TValueService, TGenom>> entites)
           where TValueService : IValueService, new() where TGenom : IGenom, new()
        {
            for (int i = 0; i < entites.Count; i++)
            {
                entites[i].Id = i + 1;
            }

            return entites;
        }
    }
}
