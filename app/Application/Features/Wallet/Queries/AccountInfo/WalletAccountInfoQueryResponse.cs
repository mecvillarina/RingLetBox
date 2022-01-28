using Application.Common.Dtos.ApiResponses;
using System.Collections.Generic;

namespace Application.Features.Wallet.Queries.AccountInfo
{
    public class WalletAccountInfoQueryResponse
    {
        public string WalletName { get; set; }
        public string Network { get; set; }
        public long CreationTime { get; set; }
        public bool IsDecrypted { get; set; }
        public long LastBlockSyncedHeight { get; set; }
        public long ChainTip { get; set; }
        public bool IsChainSynced { get; set; }
        public long ConnectedNodes { get; set; }

        public List<WalletAddressDto> Addresses { get; set; }
    }
}
