using Newtonsoft.Json;
using System.Collections.Generic;

namespace Application.Common.Dtos.ApiResponses
{
    public class SmartContractsReceiptDto
    {
        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonProperty("blockHash")]
        public string BlockHash { get; set; }

        [JsonProperty("blockNumber")]
        public long BlockNumber { get; set; }

        [JsonProperty("postState")]
        public string PostState { get; set; }

        [JsonProperty("gasUsed")]
        public long GasUsed { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("newContractAddress")]
        public string NewContractAddress { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("returnValue")]
        public string ReturnValue { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("logs")]
        public List<SmartContractsLogElementDto> Logs { get; set; }
    }

    public class SmartContractsLogElementDto
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("topics")]
        public List<string> Topics { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("log")]
        public Dictionary<string, object> Log { get; set; }
    }
}
