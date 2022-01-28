using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
using Blazored.FluentValidation;
using Client.App.Infrastructure.Managers;
using Client.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.App.Pages.Authentication
{
    public partial class Auth : IPageBase
    {
        [Inject] private IWalletManager WalletManager { get; set; }

        MudTabs tabs;

        private FluentValidationValidator LoginFluentValidationValidator;
        private bool LoginValidated => LoginFluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private LoginCommand LoginModel { get; set; } = new();

        private FluentValidationValidator RecoverFluentValidationValidator;
        private bool RecoverValidated => RecoverFluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private RecoverCommand RecoverModel { get; set; } = new();

        public bool IsLoaded { get; set; }
        public bool IsProcessing { get; set; }
        public bool IsLoginTabVisible { get; set; }
        public List<string> WalletList { get; set; } = new List<string>();

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await FetchWalletsAsync();
                RecoverModel.Passphrase = string.Empty;
                IsLoaded = true;
                StateHasChanged();
                _ = await Task.FromResult(0);
            }
        }

        private async Task FetchWalletsAsync()
        {
            try
            {
                var response = await _exceptionHandler.HandlerRequestTaskAsync(() => WalletManager.ListWalletsAsync());
                WalletList = response.Data;
                IsLoginTabVisible = WalletList.Any();
                LoginModel.Name = WalletList.FirstOrDefault();
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

        private async Task LoginAsync()
        {
            if (LoginValidated)
            {
                IsProcessing = true;

                try
                {
                    var loginResponse = await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.LoginAsync(LoginModel));
                    _appDialogService.ShowSuccess(string.Format("Welcome"));
                    await Task.Delay(1000);
                    _navigationManager.NavigateTo("/", true);
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

        private async Task RecoverAsync()
        {
            if (RecoverValidated)
            {
                IsProcessing = true;

                try
                {
                    await _exceptionHandler.HandlerRequestTaskAsync(() => _accountManager.RecoverAsync(RecoverModel));
                    _appDialogService.ShowSuccess(string.Format("You've succesfully restore your wallet."));
                    await Task.Delay(1000);

                    if (!IsLoginTabVisible)
                    {
                        _navigationManager.NavigateTo("/", true);
                    }
                    else
                    {
                        tabs.ActivatePanel(0);
                    }
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

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;
            }
        }

        private void OnActivePanelIndexChanged(int idx)
        {
            LoginModel.Name = WalletList.FirstOrDefault();
            LoginModel.Password = string.Empty;
            RecoverModel.Name = string.Empty;
            RecoverModel.Passphrase = string.Empty;
            RecoverModel.Mnemonic = string.Empty;
            RecoverModel.Password = string.Empty;
            if (_passwordVisibility)
            {
                TogglePasswordVisibility();
            }
        }

    }
}