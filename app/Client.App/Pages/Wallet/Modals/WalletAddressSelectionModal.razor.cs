using Application.Common.Dtos.ApiResponses;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Wallet.Modals
{
    public partial class WalletAddressSelectionModal : IPageBase
    {
        [Inject] private IWalletManager WalletManager { get; set; }
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        public bool IsProcessing { get; set; }
        public bool IsLoaded { get; set; }
        public List<WalletAddressDto> Addresses { get; set; } = new();
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchWalletAddressesAsync();
                IsLoaded = true;
                StateHasChanged();
                _ = await Task.FromResult(0);
            }
        }

        private async Task FetchWalletAddressesAsync()
        {
            try
            {
                var walletAccountInfo = await WalletManager.GetAccountInfoAsync();
                Addresses = walletAccountInfo.Data.Addresses.OrderByDescending(x => x.AmountConfirmed).ToList();
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }
        }

        private async Task Select(string address)
        {
            await WalletManager.SetWalletAddressAsync(address);
            _navigationManager.NavigateTo("/", true);
        }
    }
}