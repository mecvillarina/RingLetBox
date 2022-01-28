using Application.Common.Models;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IAccountWebService : IWebService
    {
        Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request);
        Task<IResult> RecoverAsync(RecoverCommand request);
    }
}