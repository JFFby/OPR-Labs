using System.Collections.Generic;

namespace OPR.lb2.Interfaces.Common
{
    public interface IGenerator
    {
        IList<T> Generate<T>(int count);
    }
}
