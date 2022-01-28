using Newtonsoft.Json;

namespace Application.Common.Dtos.ApiResponses
{
    public class GasConsumedDto
    {
        [JsonProperty("value")]
        public long Value { get; set; }
    }
}
