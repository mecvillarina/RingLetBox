using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Features.Dashboard.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebApi.Base;

namespace WebApi.Apis
{
    public class DashboardController : HttpFunctionBase
    {
        public DashboardController(IConfiguration configuration, IMediator mediator, ICallContext context) : base(configuration, mediator, context)
        {
        }

        [FunctionName("DashboardGet")]
        public async Task<IActionResult> Get([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dashboard/get")] GetDashboardInfoQuery queryArgs, HttpRequest req, ExecutionContext context)
        {
            return await ExecuteAsync<GetDashboardInfoQuery, Result<GetDashboardInfoResponse>>(context, req, queryArgs);
        }
    }
}
