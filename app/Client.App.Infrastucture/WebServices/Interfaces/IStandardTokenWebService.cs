using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Token.Commands.Add;
using Application.Features.Token.Commands.Create;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IStandardTokenWebService : IWebService
    {
        Task<IResult<List<HolderStandardTokenDto>>> GetAllHolderTokensAsync(string senderAddress, string accessToken);
        Task<IResult> CreateAsync(CreateStandardTokenCommand request, string accessToken);
        Task<IResult> AddAsync(AddStandardTokenCommand request, string accessToken);
    }
}