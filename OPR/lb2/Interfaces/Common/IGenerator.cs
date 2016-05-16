using System.Collections.Generic;
using OPR.SSGA2;

namespace OPR.lb2.Interfaces.Common
{
    public interface IGenerator<T> : IArgsGenerator
    {
        IList<T> Generate(int count);
        
        void SetupState(dynamic state);
    }

    public interface IArgsGenerator
    {
        IList<EntityArgs> GenerateEntityArgs(int count);
    }
}
