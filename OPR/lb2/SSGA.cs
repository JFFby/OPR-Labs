using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb1;
using OPR.lb2.Enums;

namespace OPR.lb2
{
    public sealed class SSGA
    {
        private readonly float[] interval_x = new float[] { -4, 4 };
        private readonly float[] interval_y = new float[] { -4, 4 };
        private readonly List<BinaryGeneration> generations = new List<BinaryGeneration>();
        private readonly byte N = 10;
        private readonly byte count = 10;

        public SSGA(float[] interval_x, float[] interval_y, byte N, byte n)
        {
            count = n;
            this.N = N;
            this.interval_x = interval_x;
            this.interval_y = interval_y;
        }

        public List<BinaryGeneration> Start()
        {
            var firstGeneration = CreateFirstPopulation();
            generations.Add(firstGeneration);
            return generations;
        }

        public List<BinaryGeneration> EvalutionStep()
        {
            var generation = generations.Last();
            var bestEntities = generation.GetBest(2);
            var children = ((BinaryEntity)bestEntities[0]).NextGeneration(bestEntities[1] as BinaryEntity,
                x => fn(x.X.Value, x.Y.Value));
            var usefunChildren = children.Where(x => IsPointValid(x.Genom.X.Value, x.Genom.Y.Value)).ToList();
            var nextGeneration = CreateNextGeneration(generation, usefunChildren);
            generation.AddChilds(children);
            generations.Add(nextGeneration);
            return generations;
        }

        private BinaryGeneration CreateNextGeneration(BinaryGeneration generation, IList<BinaryEntity> children)
        {
            if (children.Any())
            {
                var bestChild = children.First(x => Math.Abs(x.Value - children.Min(c => c.Value)) < 0.01);
                bestChild.Function = EntityFunction.BestChild;
                var nextGeneration = new List<Entity<BinaryGenom>>(generation.Winners());
                nextGeneration.Add(bestChild);
                return new BinaryGeneration(nextGeneration
                    .OfType<BinaryEntity>()
                    .Select(x => new BinaryEntity(x))
                    .OfType<Entity<BinaryGenom>>()
                    .ToList())
                    .MarkUpGenereation();
            }

            return generation;
        }

        private BinaryGeneration CreateFirstPopulation()
        {
            var entites = new List<Entity<BinaryGenom>>();
            for (int i = 0; i < count; i++)
            {
                var point = new Point<float>(
                    (float)Math.Round(RandomHelper.RandomFloat(interval_x[0], interval_x[1]), 1),
                    (float)Math.Round(RandomHelper.RandomFloat(interval_y[0], interval_y[1]), 1));
                entites.Add(new BinaryEntity(point, fn(point.x, point.y)));
            }

            return new BinaryGeneration(entites).MarkUpGenereation();
        }

        private bool IsPointValid(float x, float y)
        {
            return x > interval_x[0] && x < interval_x[1] && y > interval_y[0] && y < interval_y[1];
        }

        private float fn(float x, float y)
        {
            return (float)(Math.Pow(x, 2) + Math.Pow(y, 2));
            ///return (float)(100 * Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
            ///return (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
        }
    }
}
