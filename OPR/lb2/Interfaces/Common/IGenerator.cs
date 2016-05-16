using System.Collections.Generic;
using OPR.SSGA2;

namespace OPR.lb2.Interfaces.Common
{
    public interface IGenerator<T> : IArgsGenerator
    {
        IList<T> Generate();
        
        void SetupState(dynamic state);
    }

    public interface IArgsGenerator
    {
        IList<EntityArgs> GenerateEntityArgs();
    }
}
