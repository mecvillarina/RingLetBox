using Microsoft.AspNetCore.Components.Authorization;
using Client.App.Infrastructure.WebServices;
using Application.Common.Models;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
using Client.Infrastructure.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class AccountManager : ManagerBase, IAccountManager
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly AppRouteViewService _appRouteViewService;
        private readonly IAccountWebService _accountWebService;

        public AccountManager(IManagerToolkit managerToolkit, AppRouteViewService appRouteViewService, AuthenticationStateProvider authenticationStateProvider, IAccountWebService accountWebService)
            : base(managerToolkit)
        {
            _appRouteViewService = appRouteViewService;
            _authenticationStateProvider = authenticationStateProvider;
            _accountWebService = accountWebService;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request)
        {
            var response = await _accountWebService.LoginAsync(request);

            if (response.Succeeded)
            {
                var data = response.Data;
                await ManagerToolkit.SaveAuthTokenHandler(data);
                ((AppStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(data.Token);
                await _appRouteViewService.Populate();
                return await Result<LoginCommandResponse>.SuccessAsync(data);
            }

            return await Result<LoginCommandResponse>.FailAsync(response.Messages);
        }

        public Task<IResult> RecoverAsync(RecoverCommand request) =>  _accountWebService.RecoverAsync(request);

        //public async Task<IResult<AppUserDto>> GetProfileAsync()
        //{
        //    await PrepareForWebserviceCall();
        //    var response = await _accountWebService.GetProfileAsync(AccessToken);

        //    if (response.Succeeded)
        //    {
        //        await ManagerToolkit.SaveAccountProfile(response.Data);
        //    }

        //    return response;
        //}

        //public async Task<AppUserDto> FetchProfileAsync()
        //{
        //    return await ManagerToolkit.GetProfile();
        //}

        public async Task<IResult> LogoutAsync()
        {
            await ManagerToolkit.ClearAuthTokenHandler();
            ((AppStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            await _appRouteViewService.Populate();
            return await Result.SuccessAsync();
        }
    }
}