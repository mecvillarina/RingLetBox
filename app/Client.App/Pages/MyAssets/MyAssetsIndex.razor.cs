using Application.Common.Dtos;
using Client.App.Infrastructure.Managers;
using Client.App.Models;
using Client.App.Pages.MyAssets.Modals;
using Client.App.Pages.MyLocks.Modals;
using Client.App.Parameters;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.MyAssets
{
    public partial class MyAssetsIndex
    {
        [Inject] private IStandardTokenManager StandardTokenManager { get; set; }
        [Inject] private IWalletManager WalletManager { get; set; }

        private MudTable<HolderStandardTokenItemModel> table;
        private string SearchString { get; set; } = string.Empty;
        private int TotalItems { get; set; }

        private List<HolderStandardTokenItemModel> Tokens { get; set; } = new();

        private bool IsLoaded { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsLoaded = true;
                StateHasChanged();
                _ = await Task.FromResult(0);
            }
        }

        private async Task<TableData<HolderStandardTokenItemModel>> ServerReload(TableState state)
        {
            try
            {
                var currentWalletAddress = await WalletManager.GetWalletAddressAsync();
                var response = await _exceptionHandler.HandlerRequestTaskAsync(() => StandardTokenManager.GetAllHolderTokensAsync(currentWalletAddress));
                var data = response.Data.Select(x => new HolderStandardTokenItemModel(x)).ToList();

                await Task.Delay(300);

                data = data.Where(item =>
                {
                    if (string.IsNullOrWhiteSpace(SearchString))
                        return true;
                    if (!string.IsNullOrEmpty(item.Name) && item.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (!string.IsNullOrEmpty(item.Symbol) && item.Symbol.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (!string.IsNullOrEmpty(item.ContractAddress) && item.ContractAddress.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                }).ToList();

                TotalItems = data.Count();

                Tokens = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
                return new TableData<HolderStandardTokenItemModel>() { TotalItems = TotalItems, Items = Tokens };
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            return new TableData<HolderStandardTokenItemModel>() { TotalItems = 0, Items = new List<HolderStandardTokenItemModel>() };
        }

        private async Task GetDataAsync(string query = null)
        {
            SearchString = query;
            await table.ReloadServerData();
        }

        private async Task InvokeCreateTokenModal()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters();
            var dialog = _dialogService.Show<CreateNewTokenModal>("Create new token", parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await GetDataAsync();
            }
        }

        private async Task InvokeAddTokenModal()
        {
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters();
            var dialog = _dialogService.Show<AddTokenModal>("Add token", parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await GetDataAsync();
            }
        }

        private async Task InvokeCreateNewLockModal(HolderStandardTokenItemModel token)
        {
            var currentWalletAddress = await WalletManager.GetWalletAddressAsync();
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters();
            parameters.Add(nameof(AddNewLockModal.Model), new AddNewLockParameter()
            {
                Sender = currentWalletAddress,
                TokenContractAddress = token.ContractAddress,
                TokenInfo = $"{token.Name} - {token.Balance} {token.Symbol}",
                TotalAmount = 0,
                TokenDecimal = token.Decimal,
                UnlockDate = DateTime.Now.AddDays(1)
            });

            var dialog = _dialogService.Show<AddNewLockModal>("Add New Lock", parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await GetDataAsync();
            }
        }

    }
}