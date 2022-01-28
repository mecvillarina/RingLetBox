using Microsoft.AspNetCore.Components;
using MudBlazor;
using Client.App.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Constants;

namespace Client.App.Shared.Components
{
    public partial class WalletCard
    {
        [Inject] public IWalletManager WalletManager { get; set; }
        [Parameter] public string Class { get; set; }
        private string WalletName { get; set; }
        private string Address { get; set; }
        private string Balance { get; set; }
        private string AvatarText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var accountInfo = await WalletManager.FetchAccountInfoAsync();

            if (accountInfo != null)
            {
                WalletName = accountInfo.WalletName;
                AvatarText = WalletName[0].ToString();

                var walletAddress = await WalletManager.GetWalletAddressAsync();
                if(walletAddress != null)
                {
                    Address = walletAddress;
                    Balance = $"Balance: {accountInfo.Addresses.First(x => x.Address == walletAddress).AmountConfirmed / ClientConstants.SatoshiDivider} TCRS";
                }
            }
        }

        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ContentText), "Are you sure you want to logout?"},
                {nameof(Dialogs.Logout.ButtonText), "Logout"},
                {nameof(Dialogs.Logout.Color), Color.Error},
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Dialogs.Logout>("Logout", parameters, options);
        }
    }
}