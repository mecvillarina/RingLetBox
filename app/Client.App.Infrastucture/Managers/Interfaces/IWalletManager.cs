using Application.Common.Models;
using Application.Features.Wallet.Queries.AccountInfo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IWalletManager : IManager
    {
        Task<IResult<List<string>>> ListWalletsAsync();
        Task<IResult<WalletAccountInfoQueryResponse>> GetAccountInfoAsync();
        Task<WalletAccountInfoQueryResponse> FetchAccountInfoAsync();
        ValueTask<string> GetWalletAddressAsync();
        ValueTask SetWalletAddressAsync(string walletAddress);
    }
}