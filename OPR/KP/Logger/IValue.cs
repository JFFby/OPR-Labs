using Newtonsoft.Json;

namespace OPR.KP.Logger
{
    [JsonObject(MemberSerialization.OptOut)]
    public class LogValue
    {
        public float Value { get; set; }
    }
}
