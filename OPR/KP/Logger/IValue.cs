using Newtonsoft.Json;

namespace OPR.KP.Logger
{
    [JsonObject(MemberSerialization.OptOut)]
    public class LogValue
    {
        public float Value { get; set; }

        public float X { get; set; }

        public float Y { get; set; }
    }
}
