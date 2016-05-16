using OPR.SSGA2.Interfaces;

namespace OPR.SSGA2.Italik
{
    public sealed class BinaryValueService : IValueService
    {
        public float Value(EntityArgs args)
        {
            var binaryArgs = args as BinaryEntityArgs;
            return GlobalSettings.Fn(binaryArgs.X, binaryArgs.Y);
        }
    }
}
