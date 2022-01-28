using Newtonsoft.Json;

namespace Application.Common.Dtos.ApiRequest
{
    public class SmartContractsMethodCallRequestDto
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
}
