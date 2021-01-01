using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DesafioConexaLabs.Entities.Enum
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EPlaylistCategory
    {
        PARTY, POP, ROCK, CLASSICAL
    }
}
