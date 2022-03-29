using Microsoft.Extensions.Configuration;
using RestSharp;
using System;

namespace Customer.API.Common.Helpers
{
    public class ApiHelper : IApiHelper
    {
        public readonly IConfiguration _configuration;
        public RestClient ApiClient { get; set; }
        public ApiHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public RestClient InitializeClient(string uri)
        {
            ApiClient = new RestClient();
            ApiClient.BaseUrl = new Uri(uri);
            return ApiClient;
        }
    }
}
