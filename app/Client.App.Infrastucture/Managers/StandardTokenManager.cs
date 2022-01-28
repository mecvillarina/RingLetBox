using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Token.Commands.Add;
using Application.Features.Token.Commands.Create;
using Client.App.Infrastructure.WebServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class StandardTokenManager : ManagerBase, IStandardTokenManager
    {
        private readonly IStandardTokenWebService _standardTokenWebService;
        public StandardTokenManager(IManagerToolkit managerToolkit, IStandardTokenWebService standardTokenWebService) : base(managerToolkit)
        {
            _standardTokenWebService = standardTokenWebService;
        }

        public async Task<IResult<List<HolderStandardTokenDto>>> GetAllHolderTokensAsync(string senderAddress)
        {
            await PrepareForWebserviceCall();
            return await _standardTokenWebService.GetAllHolderTokensAsync(senderAddress, AccessToken);
        }

        public async Task<IResult> CreateAsync(CreateStandardTokenCommand request)
        {
            await PrepareForWebserviceCall();
            return await _standardTokenWebService.CreateAsync(request, AccessToken);
        }

        public async Task<IResult> AddAsync(AddStandardTokenCommand request)
        {
            await PrepareForWebserviceCall();
            return await _standardTokenWebService.AddAsync(request, AccessToken);
        }
    }
}
