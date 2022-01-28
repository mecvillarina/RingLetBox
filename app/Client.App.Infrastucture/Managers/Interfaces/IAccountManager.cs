using Application.Common.Models;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IAccountManager : IManager
    {
        Task<ClaimsPrincipal> CurrentUser();
        Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request);
        Task<IResult> RecoverAsync(RecoverCommand request);
        //Task<IResult<AppUserDto>> GetProfileAsync();
        //Task<AppUserDto> FetchProfileAsync();
        Task<IResult> LogoutAsync();
    }
}