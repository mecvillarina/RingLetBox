using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Application.Common.Exceptions;
using Application.Common.Localization;
using Application.Common.Models;
using System.Linq;
using System.Diagnostics;

namespace Infrastructure.Services
{
    public class CirrusCoreRestSharpService
    {
        private readonly IRestClient _client;
        public CirrusCoreRestSharpService(IConfiguration configuration)
        {
            var baseUrl = configuration.GetValue<string>("CirrusNodeApiEndpoint");
            _client = new RestClient(baseUrl);
        }

        public void Execute(RestRequest request)
        {
            var response = _client.Execute(request);
            ProcessUnsuccessfulResponse(response);
        }

        public T Execute<T>(RestRequest request)
        {
            var response = _client.Execute<T>(request);
            ProcessUnsuccessfulResponse(response);
            return response.Data;
        }

        private void ProcessUnsuccessfulResponse(IRestResponse response)
        {
            if (!response.IsSuccessful)
            {
                var errorResponse = JsonConvert.DeserializeObject<CirrusCoreErrorResponse>(response.Content);
                if (errorResponse?.Errors != null && errorResponse.Errors.Any())
                {
                    throw new CirrusCoreException(errorResponse.Errors.First().Message);
                }
                else
                {
                    throw new CirrusCoreException(LocalizationResource.Error_GenericServerMessage);
                }
            }
            else
            {
                Debug.WriteLine(response.Content);
            }
        }
    }
}
