using Application.Features.Lock.Commands.Claim;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.MyLocks.Modals
{
    public partial class ClaimLockTokenModal : IPageBase
    {
        [Inject] private ILockManager LockManager { get; set; }
        [Parameter] public ClaimLockCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task ClaimAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => LockManager.ClaimLockAsync(Model));
                    _appDialogService.ShowSuccess("Lock has been claimed and token has been sent to your wallet.");
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