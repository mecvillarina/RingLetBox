using Application.Common.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.Wallet.Modals;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Pages.Content
{
    public partial class Home
    {
        [Inject] private IDashboardManager DashboardManager { get; set; }
        [Inject] private IWalletManager WalletManager { get; set; }

        public string TotalCreatedTokens { get; set; } = "-";
        public string TotalTokens { get; set; } = "-";
        public string TotalTokenLocks { get; set; } = "-";

        public List<AuditTokenCreationDto> Tokens { get; set; } = new();
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CheckWalletAddressAsync();
                await GetDashboardAsync();
                StateHasChanged();
                _ = await Task.FromResult(0);
            }
        }

        public async Task CheckWalletAddressAsync()
        {
            var walletAddress = await WalletManager.GetWalletAddressAsync();
            if (string.IsNullOrEmpty(walletAddress))
            {
                var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
                _dialogService.Show<WalletAddressSelectionModal>("Select Wallet Address", options);
            }
        }

        public async Task GetDashboardAsync()
        {
            try
            {
                var response = await _exceptionHandler.HandlerRequestTaskAsync(() => DashboardManager.GetAsync());
                var data = response.Data;

                TotalCreatedTokens = string.Format("{0:N0}", data.TotalCreatedTokensCount);
                TotalTokens = string.Format("{0:N0}", data.TotalTokensCount);
                TotalTokenLocks = string.Format("{0:N0}", data.TotalTokenLocksCount);
                Tokens = data.RecentlyCreatedTokens;
            }
            catch
            {
                TotalCreatedTokens = "0";
                TotalTokens = "0";
                TotalTokenLocks = "0";
            }
        }
    }
}
