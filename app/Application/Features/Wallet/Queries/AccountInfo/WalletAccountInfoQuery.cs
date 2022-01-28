using MediatR;
using Application.Common.Interfaces;
using Application.Common.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Wallet.Queries.AccountInfo
{
    public class WalletAccountInfoQuery : IRequest<Result<WalletAccountInfoQueryResponse>>
    {
        public class WalletAccountInfoQueryHandler : IRequestHandler<WalletAccountInfoQuery, Result<WalletAccountInfoQueryResponse>>
        {
            private readonly ICallContext _callContext;
            private readonly IWalletService _walletService;
            public WalletAccountInfoQueryHandler(ICallContext callContext, IWalletService walletService)
            {
                _callContext = callContext;
                _walletService = walletService;

            }
            public Task<Result<WalletAccountInfoQueryResponse>> Handle(WalletAccountInfoQuery request, CancellationToken cancellationToken)
            {
                var generalInfo = _walletService.GeneralInfo(_callContext.WalletName);
                var addresses = _walletService.GetAddresses(_callContext.WalletName);

                var response = new WalletAccountInfoQueryResponse()
                {
                    WalletName = generalInfo.WalletName,
                    Network = generalInfo.Network,
                    CreationTime = generalInfo.CreationTime,
                    IsDecrypted = generalInfo.IsDecrypted,
                    LastBlockSyncedHeight = generalInfo.LastBlockSyncedHeight,
                    ChainTip = generalInfo.ChainTip,
                    IsChainSynced = generalInfo.IsChainSynced,
                    ConnectedNodes = generalInfo.ConnectedNodes,
                    Addresses = addresses
                };

                return Result<WalletAccountInfoQueryResponse>.SuccessAsync(response);
            }
        }
    }
}
