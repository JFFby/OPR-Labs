using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Enums;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public sealed class Entity<TValueService, TGenom> : IValue where TValueService : IValueService, new()
        where TGenom : IGenom, new() 
    {
        public  readonly EntityArgs Args;
        private readonly IGenom genom;

        private float? value;

        public Entity(EntityArgs args)
        {
            this.Args = args;
            genom = new TGenom();
            genom.Initializae(args);
            Type = EntityType.Parent;
            Function = EntityFunction.None;
        }

        public Entity(CreationResult result): this(result.Args)
        {
            Type = result.Type;
            CrossPoint = result.CrossingPoint;
        }

        public Entity(Entity<TValueService, TGenom> entity) :this(entity.Args)
        {
            Id = entity.Id;
            GenerationId = entity.GenerationId;
        } 

        public string Code { get { return genom.Code; } }

        public int? CrossPoint { get; set; }

        public EntityType Type { get; private set; }

        public EntityFunction Function { get; set; }

        public int Id { get; set; }

        public int GenerationId { get; set; }

        public float Value
        {
            get
            {
                if (!value.HasValue)
                {
                    TValueService service = new TValueService();
                    value = service.Value(Args);
                }

                return value.Value;
            }
        }

        public List<Entity<TValueService, TGenom>> CreateChildEntity(Entity<TValueService, TGenom> entity)
        {
            return genom.CreateNewGenerationEntity(entity.Args)
                .Select(result => new Entity<TValueService, TGenom>(result))
                .ToList();
        }

        public void MargAsParent()
        {
            Type = EntityType.Parent;
        }
    }
}
