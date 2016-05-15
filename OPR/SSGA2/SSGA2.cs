using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Enums;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Extension;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public sealed class SSGA2<TValueService, TGenom> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {
        private readonly ISeparator<Entity<TValueService, TGenom>> firstStepSeprator;
        private readonly ISeparator<Entity<TValueService, TGenom>> commonSeparator;
        private readonly IGenerator<EntityArgs> Generator;
        private readonly List<Generation<TValueService, TGenom>> generations;

        public SSGA2(ISeparator<Entity<TValueService, TGenom>> firstStepSeprator,
            ISeparator<Entity<TValueService, TGenom>> commonSeparator,
            IGenerator<EntityArgs> generator)
        {
            this.firstStepSeprator = firstStepSeprator;
            this.commonSeparator = commonSeparator;
            Generator = generator;
            generations = new List<Generation<TValueService, TGenom>>();
        }

        public List<Generation<TValueService, TGenom>> Start()
        {
            var firstPopulation = GenetateFirstPopulation();
            generations.Add(firstPopulation);
            return generations;
        }

        public List<Generation<TValueService, TGenom>> EvalutionStep()
        {
            var generation = generations.Last().Entites;
            var bestEntities = commonSeparator.Separate(generation, 2, true).ToList();
            var childs = bestEntities.CreateNewEntities();
            var nextGeneration = GetNextGeneration(generation, childs);
            generation.AddRange(childs);
            generations.Add(nextGeneration);
            return generations;
        }

        public Generation<TValueService, TGenom> GetNextGeneration(
            List<Entity<TValueService, TGenom>> parents,
            List<Entity<TValueService, TGenom>> childrens)
        {
            var source = GetSeparationSource(parents, childrens);
            var bestEntity = commonSeparator.Separate(source, 1, true).First();
            bestEntity.Function = EntityFunction.BestChild;
            var nextGeneration = parents.ToGeneration().Winners();
            nextGeneration.Add(bestEntity);
            return nextGeneration.ToGeneration().MarkUpGenereation(commonSeparator);
        }

        private List<Entity<TValueService, TGenom>> GetSeparationSource(
            List<Entity<TValueService, TGenom>> parents,
            List<Entity<TValueService, TGenom>> childrens)
        {
            if (!GlobalSettings.IsBestFromChildernOnly)
            {
                childrens.AddRange(parents);
            }

            return childrens;
        }

        private Generation<TValueService, TGenom> GenetateFirstPopulation()
        {
            var args = Generator.GenerateEntityArgs();
            var allEntities = args
                .Select(x => new Entity<TValueService, TGenom>(x)).ToList();
            var validEntites = firstStepSeprator.Separate(allEntities, GlobalSettings.nFromN, true);
            return validEntites.ToGeneration().MarkUpGenereation(commonSeparator);
        }
    }
}
