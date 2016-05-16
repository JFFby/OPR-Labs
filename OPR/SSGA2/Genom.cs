using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb2;
using OPR.lb2.Enums;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public abstract class Genom
    {
        protected IChromosome cromosomeCreator;
        protected int[] _code;

        public List<CreationResult> CreateNewGenerationEntity(EntityArgs partnersArgs)
        {
            //TODO: if 1... else if 2..
            var secondCode = cromosomeCreator.EntityArgsToCode(partnersArgs);

            List<CreationResult> result = new List<CreationResult>();
            if (GlobalSettings.IsCrossingFirst)
            {
                var crossingPoint = RandomHelper.Random(2, secondCode.Length - 2);
                var firstChild = CrossCode(_code, secondCode, crossingPoint);
                AddIfValid(result, firstChild, EntityType.Child, crossingPoint);
                var secondChild = CrossCode(secondCode, _code, crossingPoint);
                AddIfValid(result, secondChild, EntityType.Child, crossingPoint);

                var mutationResult = Mutation(firstChild, secondChild);
                foreach (var i in mutationResult)
                {
                    AddIfValid(result, i, EntityType.Mutant, crossingPoint);
                }
            }
            else
            {
                var mutationResult = Mutation(_code, secondCode);
                if (mutationResult.Any())
                {
                    var crossingPoint = RandomHelper.Random(2, secondCode.Length - 2);
                    foreach (var i in mutationResult)
                    {
                        AddIfValid(result, i, EntityType.Mutant, crossingPoint);
                    }

                    var firstChild = CrossCode(mutationResult[0], mutationResult[1], crossingPoint);
                    AddIfValid(result, firstChild, EntityType.Child, crossingPoint);
                    var secondChild = CrossCode(mutationResult[1], mutationResult[0], crossingPoint);
                    AddIfValid(result, secondChild, EntityType.Child, crossingPoint);
                }
            }

           
            return result;
        }

        public int[][] Mutation(params int[][] codes)
        {
            var result = new List<int[]>();
            foreach (int[] code in codes)
            {
                if (GlobalSettings.MutationChance >= RandomHelper.Random(0, 100))
                {
                    var mutatedCode = cromosomeCreator.Mutate(code);
                    result.Add(mutatedCode);
                }
            }

            return result.ToArray();
        }

        private void AddIfValid(IList<CreationResult> result, int[] code, EntityType type, int crossingPoint)
        {
            var args = cromosomeCreator.CodeToEntityArgs(code);
            if (args != null)
            {
                result.Add(new CreationResult
                {
                    Type = type,
                    Args = args,
                    CrossingPoint = crossingPoint
                });
            }
        }

        private int[] CrossCode(int[] first, int[] second, int crossingPoint)
        {
            var result = new int[second.Length];
            Array.Copy(first, result, crossingPoint);
            Array.Copy(second, crossingPoint, result, crossingPoint, second.Length - crossingPoint);

            return result;
        }
    }
}
