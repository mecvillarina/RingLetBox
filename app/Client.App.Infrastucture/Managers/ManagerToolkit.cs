using Blazored.LocalStorage;
using Application.Common.Dtos;
using Application.Common.Models;
using Client.Infrastructure.Constants;
using Client.Infrastructure.Exceptions;
using System.Threading.Tasks;
using Application.Features.Wallet.Queries.AccountInfo;

namespace Client.App.Infrastructure.Managers
{
    public class ManagerToolkit : IManagerToolkit
    {
        private readonly ILocalStorageService _localStorage;

        public ManagerToolkit(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task SaveAuthTokenHandler(AuthTokenHandler authTokenHandler) => await _localStorage.SetItemAsync(StorageConstants.Local.AuthTokenHandler, authTokenHandler);

        public async Task ClearAuthTokenHandler()
        {
            await _localStorage.RemoveItemAsync(StorageConstants.Local.WalletAccountInfo);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthTokenHandler);
            await _localStorage.RemoveItemAsync(StorageConstants.Local.WalletAddress);
        }
        public async Task<string> GetAuthToken()
        {
            var tokenHandler = await _localStorage.GetItemAsync<AuthTokenHandler>(StorageConstants.Local.AuthTokenHandler);

            if (tokenHandler == null || !tokenHandler.IsValid())
            {
                await ClearAuthTokenHandler();
                throw new SessionExpiredException();
            }

            return tokenHandler.Token;
        }

        public async Task SaveWalletAccountInfo(WalletAccountInfoQueryResponse walletAccountInfo) => await _localStorage.SetItemAsync(StorageConstants.Local.WalletAccountInfo, walletAccountInfo);
        public async Task<WalletAccountInfoQueryResponse> GetWalletAccountInfo()
        {
            var profile = await _localStorage.GetItemAsync<WalletAccountInfoQueryResponse>(StorageConstants.Local.WalletAccountInfo);
            return profile;
        }
    }
}
