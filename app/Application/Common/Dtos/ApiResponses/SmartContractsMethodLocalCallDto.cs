using Newtonsoft.Json;

namespace Application.Common.Dtos.ApiResponses
{
    public class SmartContractsMethodLocalCallDto<T>
    {
        [JsonProperty("gasConsumed")]
        public GasConsumedDto GasConsumed { get; set; }

        [JsonProperty("revert")]
        public bool Revert { get; set; }

        [JsonProperty("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonProperty("return")]
        public T Return { get; set; }

        //[JsonProperty("logs")]
        //public List<object> Logs { get; set; }
    }

}
