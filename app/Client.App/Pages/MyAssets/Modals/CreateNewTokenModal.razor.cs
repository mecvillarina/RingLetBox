using Application.Features.Token.Commands.Create;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.MyAssets.Modals
{
    public partial class CreateNewTokenModal : IPageBase
    {
        [Inject] private IStandardTokenManager StandardTokenManager { get; set; }
        [Inject] private IWalletManager WalletManager { get; set; }
        [Parameter] public CreateStandardTokenCommand Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task CreateAsync()
        {
            if (Validated)
            {
                try
                {
                    IsProcessing = true;
                    await _exceptionHandler.HandlerRequestTaskAsync(() => StandardTokenManager.CreateAsync(Model));
                    _appDialogService.ShowSuccess(string.Format("{0} ({1}) has been created.", Model.Name, Model.Symbol));
                    MudDialog.Close();
                }
                catch (ApiOkFailedException ex)
                {
                    _appDialogService.ShowErrors(ex.Messages);
                }
                catch (Exception ex)
                {
                    _appDialogService.ShowError(ex.Message);
                }

                IsProcessing = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Model.Sender = await WalletManager.GetWalletAddressAsync();
        }
    }
}