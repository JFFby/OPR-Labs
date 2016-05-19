using OPR.lb2.Enums;

namespace OPR.SSGA2
{
    public sealed class CreationResult
    {
        internal EntityArgs Args { get; set; }

        internal EntityType Type { get; set; }

        internal string CrossingPoint { get; set; }

        internal EntityFunction? Functoin { get; set; }
    }
}
