using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb1;
using OPR.lb2.Enums;

namespace OPR.lb2
{
    public sealed class BinaryEntity : Entity<BinaryGenom>
    {
        public BinaryEntity(Point<float> point, float value) : base(point)
        {
            Value = value;
            Function = EntityFunction.None;
        }

        public BinaryEntity(BinaryEntity entity)
        {
            Value = entity.Value;
            Function = EntityFunction.None;
            Id = entity.Id;
            Genom = entity.Genom;
        }

        private BinaryEntity(BinaryGenom genom, float value) : base(genom)
        {
            Value = value;
            Function = EntityFunction.None;
        }
        
        public int Id { get; set; }

        public float Value { get; private set; }

        public EntityFunction Function { get; set; }

        public IList<BinaryEntity> NextGeneration(BinaryEntity partner, Func<BinaryGenom, float> calculator)
        {
            return Genom.NexGeneration(partner.Genom)
                .Select(x => new BinaryEntity((BinaryGenom)x, calculator((BinaryGenom)x)))
                .ToList();
        }
    }
}
