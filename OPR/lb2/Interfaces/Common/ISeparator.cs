using System.Collections.Generic;

namespace OPR.lb2.Interfaces.Common
{
    public interface ISeparator
    {
        IList<T> Separate<T>(IList<T> inpuList, int count);
    }
}
