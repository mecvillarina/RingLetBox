using Application.Common.Dtos;
using Application.Common.Models;
using Application.Features.Token.Commands.Add;
using Application.Features.Token.Commands.Create;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IStandardTokenManager : IManager
    {
        Task<IResult<List<HolderStandardTokenDto>>> GetAllHolderTokensAsync(string senderAddress);
        Task<IResult> CreateAsync(CreateStandardTokenCommand request);
        Task<IResult> AddAsync(AddStandardTokenCommand request);
    }
}