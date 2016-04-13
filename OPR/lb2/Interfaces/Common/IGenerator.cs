using System.Collections.Generic;

namespace OPR.lb2.Interfaces.Common
{
    public interface IGenerator<T>
    {
        IList<T> Generate(int count, object state);
    }
}
