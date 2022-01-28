using Application.Common.Models;
using Application.Features.Dashboard.Queries.Get;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public interface IDashboardWebService : IWebService
    {
        Task<IResult<GetDashboardInfoResponse>> GetAsync();
    }
}