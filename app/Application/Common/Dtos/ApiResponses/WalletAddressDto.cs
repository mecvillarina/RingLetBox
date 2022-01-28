using Newtonsoft.Json;

namespace Application.Common.Dtos.ApiResponses
{
    public class WalletAddressDto
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("isUsed")]
        public bool IsUsed { get; set; }

        [JsonProperty("isChange")]
        public bool IsChange { get; set; }

        [JsonProperty("amountConfirmed")]
        public long AmountConfirmed { get; set; }

        [JsonProperty("amountUnconfirmed")]
        public long AmountUnconfirmed { get; set; }
    }
}
