using Newtonsoft.Json;
using Application.Common.JsonHelpers;

namespace Application.Common.Dtos.ApiResponses
{
    public class WalletGeneralInfoDto
    {
        [JsonProperty("walletName")]
        public string WalletName { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("creationTime")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long CreationTime { get; set; }

        [JsonProperty("isDecrypted")]
        public bool IsDecrypted { get; set; }

        [JsonProperty("lastBlockSyncedHeight")]
        public long LastBlockSyncedHeight { get; set; }

        [JsonProperty("chainTip")]
        public long ChainTip { get; set; }

        [JsonProperty("isChainSynced")]
        public bool IsChainSynced { get; set; }

        [JsonProperty("connectedNodes")]
        public long ConnectedNodes { get; set; }
    }
}
