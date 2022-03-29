using Customer.API.Common.Communication;
using Customer.API.Data.Entities;
using Customer.API.Service.Dtos.Response;
using Customer.API.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.XUnitTest.Services
{
    public class LgaServiceTest : ILgaService
    {
        private readonly List<LGA> _dbLga;

        public LgaServiceTest()
        {
            _dbLga = new List<LGA>()
            {
                new LGA() {Id = 1, Name = "Lagos Island", StateId = 1},
                new LGA() {Id = 2, Name = "Agege", StateId = 1},
                new LGA() {Id = 3, Name = "Delta", StateId = 2},
            };
        }

        public async Task<ExecutedResult<List<LgaResponseDto>>> Lgas(long stateId)
        {
            try
            {
                if (stateId < 0) return ExecutedResult<List<LgaResponseDto>>.BadRequest("Invalid Request");

                var result = _dbLga.FindAll(s => s.Id == stateId);
                if(result.Any())
                {
                    var response = result.Select(s => new LgaResponseDto
                    {
                        Id = s.Id,
                        Name = s.Name                        
                    }).ToList();

                    return await Task.FromResult(ExecutedResult<List<LgaResponseDto>>.Success(response, "Request successful"));
                }
                return await Task.FromResult(ExecutedResult<List<LgaResponseDto>>.NotFound("Request successful. No record found"));
            }
            catch (Exception)
            {
                return ExecutedResult<List<LgaResponseDto>>.Exception("Something went wrong");
            }
        }
    }
}
