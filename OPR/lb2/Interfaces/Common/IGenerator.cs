using System.Collections.Generic;
using OPR.SSGA2;

namespace OPR.lb2.Interfaces.Common
{
    public interface IGenerator<T>
    {
        IList<T> Generate();

        IList<EntityArgs> GenerateEntityArgs();

        void SetupState(dynamic state);
    }
}
