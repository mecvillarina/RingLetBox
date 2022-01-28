using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Wallet.Queries.AccountInfo;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IManagerToolkit : IManager
    {
        Task ClearAuthTokenHandler();
        Task SaveAuthTokenHandler(AuthTokenHandler authTokenHandler);
        Task<string> GetAuthToken();
        Task SaveWalletAccountInfo(WalletAccountInfoQueryResponse walletAccountInfo);
        Task<WalletAccountInfoQueryResponse> GetWalletAccountInfo();
    }
}