using OPR.lb1;
using System.Collections.Generic;

namespace OPR.lb2.Interfaces.Common
{
    public interface ISelection
    {
        IList<BinaryEntity> getChildOfRoulette(IList<BinaryEntity> binaryGeneration);

        IList<BinaryEntity> getChildOfRang(IList<BinaryEntity> binaryGeneration);

        IList<BinaryEntity> getChildOfTournament(IList<BinaryEntity> binaryGeneration);
    }
}
