﻿using Application.Features.Lock.Commands.RefundRevoked;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.MyLocks.Modals
{
    public partial class RefundRevokedLockTokenModal : IPageBase
    {
        [Inject] private ILockManager LockManager { get; set; }
        [Parameter] public RefundRevokedLockCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task OkAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => LockManager.RefundRevokedLockAsync(Model));
                    _appDialogService.ShowSuccess("Token has been refunded.");
                }
                catch (ApiOkFailedException ex)
                {
                    _appDialogService.ShowErrors(ex.Messages);
                }
                catch (Exception ex)
                {
                    _appDialogService.ShowError(ex.Message);
                }
                MudDialog.Close();
                IsProcessing = false;
            }
        }
    }
}