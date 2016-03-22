using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb1;
using OPR.lb2.Enums;
using OPR.lb2.Interfaces;

namespace OPR.lb2
{
    public sealed class BinaryGenom : IGenom
    {
        public readonly int? CrossingPoint;
        public BinaryСhromosome X;
        public BinaryСhromosome Y;

        public BinaryGenom() { }

        public BinaryGenom(Point<float> point)
        {
            X = new BinaryСhromosome(point.x);
            Y = new BinaryСhromosome(point.y);
            Type = EntityType.Parent;
        }

        public BinaryGenom(BinaryСhromosome x, BinaryСhromosome y, int? crossingPoint)
        {
            X = x;
            Y = y;
            CrossingPoint = crossingPoint;
            Type = EntityType.Child;
        }

        public BinaryGenom(BinaryСhromosome x, BinaryСhromosome y, EntityType type) : this(x, y, null)
        {
            Type = type;
        }


        public string Code
        {
            get { return X.Code + Y.Code; }
        }

        public EntityType Type { get; private set; }

        public void Initialize(Point<float> point)
        {
            X = new BinaryСhromosome(point.x);
            Y = new BinaryСhromosome(point.y);
        }

        public IList<IGenom> NexGeneration(IGenom partner)
        {
            var binatyPartner = (BinaryGenom)partner;
            var entities = Crossig(binatyPartner.Code);
            return Mutation(entities);
        }

        private IGenom[] Crossig(string anotherCode)
        {
            var crossingPoint = RandomHelper.Random(2, anotherCode.Length - 2);
            var firstCode = anotherCode.Substring(0, crossingPoint)
                            + Code.Substring(crossingPoint, anotherCode.Length - crossingPoint);
            var secondCode = Code.Substring(0, crossingPoint)
                             + anotherCode.Substring(crossingPoint, anotherCode.Length - crossingPoint);
            var chromosomeMiddlee = (int)Math.Floor((double)anotherCode.Length / 2);

            var c1 = new BinaryGenom(
                new BinaryСhromosome(firstCode.Substring(0, chromosomeMiddlee)),
                new BinaryСhromosome(firstCode.Substring(chromosomeMiddlee, chromosomeMiddlee)), crossingPoint);
            var c2 = new BinaryGenom(
                new BinaryСhromosome(secondCode.Substring(0, chromosomeMiddlee)),
                new BinaryСhromosome(secondCode.Substring(chromosomeMiddlee, chromosomeMiddlee)), crossingPoint);

            return new IGenom[] { c1, c2 };
        }

        public IList<IGenom> Mutation(IGenom[] entities)
        {
            var result = new List<IGenom>(entities);

            foreach (var entity in entities.OfType<BinaryGenom>())
            {
                string x_mutant = BinaryСhromosome.Mutate(entity.X.Code);
                string y_mutant = BinaryСhromosome.Mutate(entity.Y.Code);
                if (x_mutant != null || y_mutant != null)
                {
                    result.Add(new BinaryGenom(
                        x_mutant == null
                            ? entity.X
                            : new BinaryСhromosome(x_mutant),
                        y_mutant == null
                            ? entity.Y
                            : new BinaryСhromosome(y_mutant),
                        EntityType.Mutant));
                }
            }
            return result;
        }
    }
}
