using Customer.API.Common.Communication;
using Customer.API.Service.Dtos.Response;
using Customer.API.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.XUnitTest.Services
{
    public class GoldServiceTest : IGoldService
    {
        private readonly List<GoldPriceResponseDto> _listData;

        public GoldServiceTest()
        {
            _listData = new List<GoldPriceResponseDto>()
            {
                new GoldPriceResponseDto() {price = "$1000.00"},
                new GoldPriceResponseDto() {price = "$500.00"},
            };
        }

        public async Task<ExecutedResult<List<GoldPriceResponseDto>>> GoldPrice()
        {
            try
            {
                var data = _listData;               
                if(data.Any())
                {
                    return await Task.FromResult(ExecutedResult<List<GoldPriceResponseDto>>.Success(data, "Request successful"));
                }           
                return ExecutedResult<List<GoldPriceResponseDto>>.Failed(data, "Request failed");
            }
            catch (Exception)
            {
                return ExecutedResult<List<GoldPriceResponseDto>>.Exception("Something went wrong");
            }
        }
    }
}
