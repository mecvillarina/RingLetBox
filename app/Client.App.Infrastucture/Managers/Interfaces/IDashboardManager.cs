using Application.Common.Models;
using Application.Features.Dashboard.Queries.Get;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<GetDashboardInfoResponse>> GetAsync();
    }
}