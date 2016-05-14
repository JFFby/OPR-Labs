using System.Collections.Generic;

namespace OPR.SSGA2.Interfaces
{
    public interface IGenom
    {
        string Code { get; set; }

        List<CreationResult> CreateNewGenerationEntity(EntityArgs partnersArgs);

        void Initializae(EntityArgs args);
    }
}
