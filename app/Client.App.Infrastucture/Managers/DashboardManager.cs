using Application.Common.Models;
using Application.Features.Dashboard.Queries.Get;
using Client.App.Infrastructure.WebServices;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Managers
{
    public class DashboardManager : ManagerBase, IDashboardManager
    {
        private readonly IDashboardWebService _dashboardWebService;

        public DashboardManager(IManagerToolkit managerToolkit, IDashboardWebService dashboardWebService) : base(managerToolkit)
        {
            _dashboardWebService = dashboardWebService;
        }

        public Task<IResult<GetDashboardInfoResponse>> GetAsync() => _dashboardWebService.GetAsync();
    }
}
