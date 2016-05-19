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
            var secondCode = cromosomeCreator.EntityArgsToCode(partnersArgs);
            List<CreationResult> result = new List<CreationResult>();

            if (GlobalSettings.IsTwoCrossingPoints)
            {
                TwoPointCrossing(result, secondCode);
            }
            else
            {
                OnePointCrossing(result, secondCode);
            }

            return result;
        }

        public int[][] Mutation(params int[][] codes)
        {
            var result = new List<int[]>();
            foreach (int[] code in codes)
            {
                var chance = RandomHelper.Random(0, 100);
                if (GlobalSettings.MutationChance >= chance)
                {
                    var mutatedCode = cromosomeCreator.Mutate(code);
                    result.Add(mutatedCode);
                }
                else
                {
                    result.Add(new int[] { });
                }
            }

            return result.ToArray();
        }

        private void TwoPointCrossing(List<CreationResult> result, int[] secondCode)
        {
            var cp1 = RandomHelper.Random(1, secondCode.Length - 2);
            var cp2 = RandomHelper.Random(cp1 + 1, secondCode.Length - 1);
            var modelCp = string.Join("_", cp1, cp2);
            if (GlobalSettings.IsCrossingFirst)
            {
                var firstChild = TpCroosCode(_code, secondCode, cp1, cp2);
                AddIfValid(result, firstChild, EntityType.Child, modelCp);
                var secondChild = TpCroosCode(secondCode, _code, cp1, cp2);
                AddIfValid(result, secondChild, EntityType.Child, modelCp);
                var mutationResult = Mutation(firstChild, secondChild);
                foreach (var i in mutationResult)
                {
                    if (i.Length > 0)
                    {
                        AddIfValid(result, i, EntityType.Mutant, modelCp);
                    }
                }
            }
            else
            {
                var mutationResult = Mutation(_code, secondCode);
                if (mutationResult.Any())
                {
                    foreach (var i in mutationResult)
                    {
                        if (i.Length > 0)
                        {
                            AddIfValid(result, i, EntityType.Mutant, modelCp);
                        }
                    }
                }

                mutationResult = GetCodesForCrosing(mutationResult, new int[][] { _code, secondCode });
                var firstChild = TpCroosCode(mutationResult[0], mutationResult[1], cp1, cp2);
                AddIfValid(result, firstChild, EntityType.Child, modelCp);
                var secondChild = TpCroosCode(mutationResult[1], mutationResult[0], cp1, cp2);
                AddIfValid(result, secondChild, EntityType.Child, modelCp);
            }

        }

        private int[][] GetCodesForCrosing(int[][] mutationResults, int[][] codes)
        {
            var result = new List<int[]>();
            for (int i = 0; i < codes.Length; i++)
            {
                if (mutationResults[i].Length > 0)
                {
                    result.Add(mutationResults[i]);
                }
                else
                {
                    result.Add(codes[i]);
                }
            }

            return result.ToArray();
        }

        protected int[] TpCroosCode(int[] first, int[] second, int cp1, int cp2)
        {
            var result = new int[first.Length];
            Array.Copy(first, result, cp1);
            Array.Copy(second, cp1, result, cp1, cp2 - cp1);
            Array.Copy(first, cp2, result, cp2, first.Length - cp2);

            return result;
        }

        private void OnePointCrossing(List<CreationResult> result, int[] secondCode)
        {
            if (GlobalSettings.IsCrossingFirst)
            {
                var crossingPoint = RandomHelper.Random(2, secondCode.Length - 2);
                var firstChild = CrossCode(_code, secondCode, crossingPoint);
                AddIfValid(result, firstChild, EntityType.Child, crossingPoint.ToString());
                var secondChild = CrossCode(secondCode, _code, crossingPoint);
                AddIfValid(result, secondChild, EntityType.Child, crossingPoint.ToString());

                var mutationResult = Mutation(firstChild, secondChild);
                foreach (var i in mutationResult)
                {
                    if (i.Length > 0)
                    {
                        AddIfValid(result, i, EntityType.Mutant, crossingPoint.ToString());
                    }
                }
            }
            else
            {
                var mutationResult = Mutation(_code, secondCode);
                var crossingPoint = RandomHelper.Random(2, secondCode.Length - 2);
                if (mutationResult.Any())
                {
                    foreach (var i in mutationResult)
                    {
                        if (i.Length > 0)
                        {
                            AddIfValid(result, i, EntityType.Mutant, crossingPoint.ToString());
                        }
                    }

                }

                mutationResult = GetCodesForCrosing(mutationResult, new int[][] { _code, secondCode });
                var firstChild = CrossCode(mutationResult[0], mutationResult[1], crossingPoint);
                AddIfValid(result, firstChild, EntityType.Child, crossingPoint.ToString());
                var secondChild = CrossCode(mutationResult[1], mutationResult[0], crossingPoint);
                AddIfValid(result, secondChild, EntityType.Child, crossingPoint.ToString());
            }
        }

        private void AddIfValid(IList<CreationResult> result, int[] code, EntityType type, string crossingPoint)
        {
            var validationResult = cromosomeCreator.CodeToEntityArgs(code);
            if (validationResult != null)
            {
                result.Add(new CreationResult
                {
                    Type = type,
                    Args = validationResult.Args,
                    CrossingPoint = crossingPoint,
                    Functoin = validationResult.IsValid ? (EntityFunction?)null : EntityFunction.NotValid
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
