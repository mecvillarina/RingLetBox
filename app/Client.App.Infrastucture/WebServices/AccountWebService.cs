using Application.Common.Models;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Recover;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class AccountWebService : WebServiceBase, IAccountWebService
    {
        public AccountWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<LoginCommandResponse>> LoginAsync(LoginCommand request) => PostAsync<LoginCommand, LoginCommandResponse>(AuthEndpoints.Login, request);
        public Task<IResult> RecoverAsync(RecoverCommand request) => PostAsync(AuthEndpoints.Recover, request);
    }
}
