using Application.Common.Dtos;
using Application.Features.Lock.Commands.Claim;
using Application.Features.Lock.Commands.RefundRevoked;
using Application.Features.Lock.Commands.Revoke;
using Client.App.Infrastructure.Managers;
using Client.App.Pages.MyLocks.Modals;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.MyLocks
{
    public partial class MyLocksIndex
    {
        [Inject] private IWalletManager WalletManager { get; set; }
        [Inject] private ILockManager LockManager { get; set; }

        private MudTable<LockTransactionWithTypeDto> table;
        private string SearchString { get; set; } = string.Empty;
        private int TotalItems { get; set; }

        private List<LockTransactionWithTypeDto> LockTransactions { get; set; } = new();

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

        private async Task<TableData<LockTransactionWithTypeDto>> ServerReload(TableState state)
        {
            try
            {
                var currentWalletAddress = await WalletManager.GetWalletAddressAsync();
                var response = await _exceptionHandler.HandlerRequestTaskAsync(() => LockManager.GetAllBySenderAsync(currentWalletAddress));
                var data = response.Data.ToList();

                await Task.Delay(300);

                data = data.Where(item =>
                {
                    if (string.IsNullOrWhiteSpace(SearchString))
                        return true;
                    if (!string.IsNullOrEmpty(item.TokenSymbol) && item.TokenSymbol.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (!string.IsNullOrEmpty(item.TokenName) && item.TokenName.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (!string.IsNullOrEmpty(item.TokenAddress) && item.TokenAddress.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                }).ToList();

                TotalItems = data.Count();

                LockTransactions = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
                return new TableData<LockTransactionWithTypeDto>() { TotalItems = TotalItems, Items = LockTransactions };
            }
            catch (ApiOkFailedException ex)
            {
                _appDialogService.ShowErrors(ex.Messages);
            }
            catch (Exception ex)
            {
                _appDialogService.ShowError(ex.Message);
            }

            return new TableData<LockTransactionWithTypeDto>() { TotalItems = 0, Items = new List<LockTransactionWithTypeDto>() };
        }

        private async Task GetDataAsync(string query = null)
        {
            SearchString = query;
            await table.ReloadServerData();
        }

        private async Task InvokeRevokeLockModal(LockTransactionWithTypeDto transaction)
        {
            var currentWalletAddress = await WalletManager.GetWalletAddressAsync();
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters();
            parameters.Add(nameof(RevokeLockTokenModal.Model), new RevokeLockCommand()
            {
                Sender = currentWalletAddress,
                LockTransactionIndex = transaction.LockTransactionIndex
            });

            var dialog = _dialogService.Show<RevokeLockTokenModal>("Revoke Lock Confirmation", parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await GetDataAsync();
            }
        }

        private async Task InvokeRefundRevokedLockModal(LockTransactionWithTypeDto transaction)
        {
            var currentWalletAddress = await WalletManager.GetWalletAddressAsync();
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters();
            parameters.Add(nameof(RefundRevokedLockTokenModal.Model), new RefundRevokedLockCommand()
            {
                Sender = currentWalletAddress,
                LockTransactionIndex = transaction.LockTransactionIndex
            });

            var dialog = _dialogService.Show<RefundRevokedLockTokenModal>("Refund Revoked Lock Confirmation", parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await GetDataAsync();
            }
        }

        private async Task InvokeClaimLockModal(LockTransactionWithTypeDto transaction)
        {
            var currentWalletAddress = await WalletManager.GetWalletAddressAsync();
            var options = new DialogOptions { CloseButton = false, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var parameters = new DialogParameters();
            parameters.Add(nameof(ClaimLockTokenModal.Model), new ClaimLockCommand()
            {
                Sender = currentWalletAddress,
                LockTransactionIndex = transaction.LockTransactionIndex
            });

            var dialog = _dialogService.Show<ClaimLockTokenModal>("Claim Lock Confirmation", parameters, options);
            var dialogResult = await dialog.Result;
            if (!dialogResult.Cancelled)
            {
                await GetDataAsync();
            }
        }
    }
}