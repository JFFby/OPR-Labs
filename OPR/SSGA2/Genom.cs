using System;
using System.Collections.Generic;
using OPR.lb2;
using OPR.lb2.Enums;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public abstract class Genom
    {
        private readonly int mutationChance = 7; //TODO: To global settings

        protected IChromosome cromosomeCreator;
        protected int[] _code;

        public List<CreationResult> CreateNewGenerationEntity(EntityArgs partnersArgs)
        {
            //TODO: if 1... else if 2..
            var secondCode = cromosomeCreator.EntityArgsToCode(partnersArgs);
            var crossingPoint = RandomHelper.Random(2, secondCode.Length - 2);
            var firstChild = CrossCode(_code, secondCode, crossingPoint);
            var secondChild = CrossCode(secondCode, _code, crossingPoint);
            return Mutation(firstChild, secondChild);
        }

        public List<CreationResult> Mutation(params int[][] codes)
        {
            var result = new List<CreationResult>();
            foreach (int[] code in codes)
            {
                if (mutationChance >= RandomHelper.Random(0, 100))
                {
                    var mutatedCode = cromosomeCreator.Mutate(code);
                    AddIfValid(result, mutatedCode);
                }
                else
                {
                    AddIfValid(result, code);
                }
            }

            return result;
        }

        private void AddIfValid(IList<CreationResult> result, int[] code)
        {
            var args = cromosomeCreator.CodeToEntityArgs(code);
            if (args != null)
            {
                result.Add(new CreationResult
                {
                    Type = EntityType.Mutant,
                    Args = args
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
