using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Enums;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2
{
    public sealed class Entity<TValueService, TGenom> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {
        private readonly EntityArgs args;
        private readonly IGenom genom;

        private float? value;

        public Entity(EntityArgs args)
        {
            this.args = args;
            genom = new TGenom();
            genom.Initializae(args);
            Type = EntityType.Parent;
            Function = EntityFunction.None;
        }

        public Entity(CreationResult result): this(result.Args)
        {
            Type = result.Type;
        } 

        public string Code { get { return genom.Code; } }

        public EntityType Type { get; private set; }

        public EntityFunction Function { get; set; }

        public float Value
        {
            get
            {
                if (!value.HasValue)
                {
                    TValueService service = new TValueService();
                    value = service.Value(args);
                }

                return value.Value;
            }
        }

        public List<Entity<TValueService, TGenom>> CreateChildEntity(Entity<TValueService, TGenom> entity)
        {
            return genom.CreateNewGenerationEntity(entity.args)
                .Select(result => new Entity<TValueService, TGenom>(result))
                .ToList();
        }
    }
}
