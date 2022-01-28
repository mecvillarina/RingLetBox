using Application.Common.Models;
using Application.Features.Wallet.Queries.AccountInfo;
using Blazored.LocalStorage;
using Client.App.Infrastructure.WebServices;
using Client.Infrastructure.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class WalletManager : ManagerBase, IWalletManager
    {
        private readonly IWalletWebService _walletWebService;
        private readonly ILocalStorageService _localStorage;

        public WalletManager(IManagerToolkit managerToolkit, IWalletWebService walletWebService, ILocalStorageService localStorage) : base(managerToolkit)
        {
            _walletWebService = walletWebService;
            _localStorage = localStorage;
        }

        public Task<IResult<List<string>>> ListWalletsAsync() => _walletWebService.ListWalletsAsync();
        public async Task<IResult<WalletAccountInfoQueryResponse>> GetAccountInfoAsync()
        {
            await PrepareForWebserviceCall();
            var response = await _walletWebService.AccountInfoAsync(AccessToken);
            if (response.Succeeded)
            {
                await ManagerToolkit.SaveWalletAccountInfo(response.Data);
            }
            return response;
        }

        public async Task<WalletAccountInfoQueryResponse> FetchAccountInfoAsync()
        {
            return await ManagerToolkit.GetWalletAccountInfo();
        }

        public ValueTask<string> GetWalletAddressAsync() => _localStorage.GetItemAsync<string>(StorageConstants.Local.WalletAddress);
        public ValueTask SetWalletAddressAsync(string walletAddress) => _localStorage.SetItemAsStringAsync(StorageConstants.Local.WalletAddress, walletAddress);
    }
}
