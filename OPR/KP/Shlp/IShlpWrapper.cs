using OPR.lb1;

namespace OPR.KP.Shlp
{
    public interface IShlpWrapper
    {
        IShlp GetShlpObject(SquarePoint point);

        ShlpType ShlpType { get; }
    }
}
