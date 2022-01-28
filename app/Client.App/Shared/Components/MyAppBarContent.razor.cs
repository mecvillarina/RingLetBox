using Microsoft.AspNetCore.Components;
using MudBlazor;
using Client.App.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Constants;
using Client.App.Pages.Wallet.Modals;
using Client.App.PeriodicExecutor;

namespace Client.App.Shared.Components
{
    public partial class MyAppBarContent
    {
        [Inject] public IWalletManager WalletManager { get; set; }
        private string WalletName { get; set; }
        private string Address { get; set; }
        private string Balance { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            WalletExecutor.JobExecuted += HandleJobExecuted;
        }

        public void Dispose()
        {
            WalletExecutor.JobExecuted -= HandleJobExecuted;
        }

        void HandleJobExecuted(object sender, WalletJobExecutedEventArgs e)
        {
            Console.WriteLine("UI");
            WalletName = e.WalletName;
            Address = e.WalletAddress;
            Balance = e.WalletBalance;
            StateHasChanged();
        }

        private async Task LoadDataAsync()
        {
            var accountInfo = await WalletManager.FetchAccountInfoAsync();

            if (accountInfo != null)
            {
                WalletName = accountInfo.WalletName;

                var walletAddress = await WalletManager.GetWalletAddressAsync();
                if(walletAddress != null)
                {
                    Address = walletAddress;
                    var balance = (double)accountInfo.Addresses.First(x => x.Address == walletAddress).AmountConfirmed / ClientConstants.SatoshiDivider;
                    Balance = string.Format("{0:N2} TCRS", balance);
                }
            }
        }

        private void SwitchWalletAddress()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            _dialogService.Show<WalletAddressSelectionModal>("Select Wallet Address", options);
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