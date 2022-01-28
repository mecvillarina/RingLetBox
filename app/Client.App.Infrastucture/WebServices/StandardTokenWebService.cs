using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Token.Commands.Add;
using Application.Features.Token.Commands.Create;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class StandardTokenWebService : WebServiceBase, IStandardTokenWebService
    {
        public StandardTokenWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<List<HolderStandardTokenDto>>> GetAllHolderTokensAsync(string sender, string accessToken) => GetAsync<List<HolderStandardTokenDto>>(string.Format(StandardTokenEndpoints.GetAllHolderTokens, sender), accessToken);
        public Task<IResult> CreateAsync(CreateStandardTokenCommand request, string accessToken) => PostAsync<CreateStandardTokenCommand>(StandardTokenEndpoints.Create, request, accessToken);
        public Task<IResult> AddAsync(AddStandardTokenCommand request, string accessToken) => PostAsync<AddStandardTokenCommand>(StandardTokenEndpoints.Add, request, accessToken);
    }
}
