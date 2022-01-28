using Application.Features.Lock.Commands.Add;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.App.Models;
using Client.App.Parameters;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace Client.App.Pages.MyLocks.Modals
{
    public partial class AddNewLockModal : IPageBase
    {
        [Inject] private ILockManager LockManager { get; set; }

        [Parameter] public AddNewLockParameter Model { get; set; } = new();
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        public bool IsProcessing { get; set; }
        public bool IsLoading { get; set; }
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

                    var command = new AddNewLockCommand()
                    {
                        DurationInDays = Model.UnlockDate.Value.Date.Subtract(DateTime.Now.Date).Days,
                        IsRevocable = Model.IsRevocable,
                        RecipientAddress = Model.RecipientAddress,
                        Sender = Model.Sender,
                        TokenContractAddress = Model.TokenContractAddress,
                        TotalAmount = $"{(long)(Model.TotalAmount * Math.Pow(10, Model.TokenDecimal))}"
                    };

                    await _exceptionHandler.HandlerRequestTaskAsync(() => LockManager.AddNewLockAsync(command));
                    _appDialogService.ShowSuccess("New lock has been created.");
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

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Model.UnlockDate = DateTime.Now.AddDays(1);
        }
    }
}