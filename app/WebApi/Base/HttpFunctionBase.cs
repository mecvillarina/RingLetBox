using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Localization;
using Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnauthorizedAccessException = Application.Common.Exceptions.UnauthorizedAccessException;

namespace WebApi.Base
{
    public abstract class HttpFunctionBase
    {
        protected readonly IConfiguration Configuration;
        protected readonly ICallContext Context;
        protected readonly IMediator Mediator;
        protected HttpFunctionBase(IConfiguration configuration, IMediator mediator, ICallContext context)
        {
            Configuration = configuration;
            Mediator = mediator;
            Context = context;
        }

        protected void EnsureAuthorization(HttpRequest httpRequest)
        {
            Context.UserRequiresAuthorization = true;

            var authHeaderName = "Authorization";
            var bearerPrefix = "Bearer ";

            if (httpRequest != null &&
                httpRequest.Headers.ContainsKey(authHeaderName) &&
                httpRequest.Headers[authHeaderName].ToString().StartsWith(bearerPrefix))
            {
                Context.UserBearerAuthorizationToken = httpRequest.Headers[authHeaderName].ToString().Substring(bearerPrefix.Length);
            }
        }

        protected async Task<IActionResult> ExecuteAsync<TRequest, TResponse>(ExecutionContext executionContext,
                HttpRequest httpRequest,
                TRequest request,
                Func<TResponse, Task<IActionResult>> resultMethod = null)
                where TRequest : IRequest<TResponse>
        {
            try
            {
                Context.CorrelationId = executionContext.InvocationId;
                Context.FunctionName = executionContext.FunctionName;
                Context.UserName = httpRequest.HttpContext.User?.Identity?.Name;
                Context.AuthenticationType = httpRequest.HttpContext.User?.Identity?.AuthenticationType;
                Context.PartitionKey = Configuration.GetValue<string>("PartitionKey");

                var response = await Mediator.Send(request);

                if (resultMethod != null)
                    return await resultMethod(response);

                return new OkObjectResult(response);
            }
            catch (ValidationException validationException)
            {
                var errors = new List<string>();
                foreach (var validationError in validationException.Errors) errors.AddRange(validationError.Value);

                var result = await Result.FailAsync(errors);
                return new OkObjectResult(result);
            }
            catch (NotFoundException)
            {
                var result = await Result.FailAsync("The specified resource was not found.");
                return new OkObjectResult(result);
            }
            catch (CirrusCoreException ex)
            {
                var result = await Result.FailAsync(ex.Message);
                return new OkObjectResult(result);
            }
            catch (ForbiddenAccessException)
            {
                var result = await Result.FailAsync("You don't have access to this feature.");
                return new OkObjectResult(result);
            }
            catch (UnauthorizedAccessException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception ex)
            {
                var result = await Result.FailAsync(LocalizationResource.Error_GenericServerMessage);
                return new OkObjectResult(result);

            }
        }
    }
}