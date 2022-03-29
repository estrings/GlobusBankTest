using Customer.API.Common.Communication;
using Customer.API.Common.Helpers;
using Customer.API.Service.Dtos.Response;
using Customer.API.Service.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Customer.API.Service.Implementation
{
    public class GoldService : IGoldService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoldService> _logger;
        private readonly IApiHelper _apiHelper;
        public GoldService(IConfiguration configuration, ILogger<GoldService> logger, IApiHelper apiHelper)
        {
            _configuration = configuration;
            _logger = logger;
            _apiHelper = apiHelper;
        }

        public async Task<ExecutedResult<List<GoldPriceResponseDto>>> GoldPrice()
        {
            try
            {
                List<GoldPriceResponseDto> data = new List<GoldPriceResponseDto>();
                string url = _configuration.GetSection("API").GetSection("url").Value;
                string apiHost = _configuration.GetSection("API").GetSection("host").Value;
                string apiKey = _configuration.GetSection("API").GetSection("key").Value;

                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("X-RapidAPI-Host", apiHost);
                request.AddHeader("X-RapidAPI-Key", apiKey);
                RestClient client = _apiHelper.InitializeClient(url);

                var response = await client.ExecuteAsync(request);
                if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
                {
                    data = JsonConvert.DeserializeObject<List<GoldPriceResponseDto>>(response.Content);
                    return ExecutedResult<List<GoldPriceResponseDto>>.Success(data, "Request successful");
                }
                return ExecutedResult<List<GoldPriceResponseDto>>.Failed(data, "Request failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GoldService: An error occured when getting prices of Gold");
                return ExecutedResult<List<GoldPriceResponseDto>>.Exception("Something went wrong");
            }
        }
    }
}
