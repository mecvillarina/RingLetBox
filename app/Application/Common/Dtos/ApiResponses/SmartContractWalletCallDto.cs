using Newtonsoft.Json;

namespace Application.Common.Dtos.ApiResponses
{
    public class SmartContractWalletCallDto
    {
        [JsonProperty("fee")]
        public long Fee { get; set; }

        [JsonProperty("hex")]
        public string Hex { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
}
