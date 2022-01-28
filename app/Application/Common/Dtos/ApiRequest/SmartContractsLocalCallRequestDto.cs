using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Common.Dtos.ApiRequest
{
    public class SmartContractsLocalCallRequestDto
    {
        [JsonProperty("methodName")]
        public string MethodName { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("gasPrice")]
        public long GasPrice { get; set; }

        [JsonProperty("gasLimit")]
        public long GasLimit { get; set; }

        [JsonProperty("parameters")]
        public List<string> Parameters { get; set; } = new List<string>();

        [JsonProperty("contractAddress")]
        public string ContractAddress { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }
    }
}
