using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Common.Dtos.ApiRequest
{
    public class SmartContractWalletCreateRequestDto
    {
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("feeAmount")]
        public double FeeAmount { get; set; }

        [JsonProperty("gasPrice")]
        public long GasPrice { get; set; }

        [JsonProperty("gasLimit")]
        public long GasLimit { get; set; }

        [JsonProperty("parameters")]
        public List<string> Parameters { get; set; } = new List<string>();

        [JsonProperty("contractCode")]
        public string ContractCode { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("walletName")]
        public string WalletName { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }
    }
}
