using Application.Common.Models;
using Application.Features.Dashboard.Queries.Get;
using Client.App.Infrastructure.Routes;
using Client.App.Infrastructure.WebServices.Base;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.WebServices
{
    public class DashboardWebService : WebServiceBase, IDashboardWebService
    {
        public DashboardWebService(AppHttpClient appHttpClient) : base(appHttpClient)
        {
        }

        public Task<IResult<GetDashboardInfoResponse>> GetAsync() => GetAsync<GetDashboardInfoResponse>(DashboardEndpoints.Get);
    }
}
