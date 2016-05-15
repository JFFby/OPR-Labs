using System.Collections.Generic;

namespace OPR.lb2.Interfaces.Common
{
    public interface ISeparator<T>
    {
        IList<T> Separate(IList<T> inpuList, int count, bool isAscending);
    }
}
