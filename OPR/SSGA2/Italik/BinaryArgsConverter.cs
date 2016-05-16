using OPR.lb1;
using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2.Italik
{
    public sealed class BinaryArgsConverter : IConverter
    {
        public EntityArgs Convert(SquarePoint point)
        {
            return new BinaryEntityArgs
            {
                X = point.x,
                Y = point.y
            };
        }
    }
}
