using Newtonsoft.Json;

namespace Application.Common.Dtos
{
    public class StandardTokenAllowanceRequestDto
    {
        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("spender")]
        public string Spender { get; set; }
    }
}
