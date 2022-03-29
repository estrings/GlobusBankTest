using Customer.API.Common.Communication;
using Customer.API.Data.Entities;
using Customer.API.Data.UnitOfWork;
using Customer.API.Service.Dtos.Response;
using Customer.API.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.API.Service.Implementation
{
    public class LgaService : ILgaService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<StateService> _logger;

        public LgaService(IUnitofWork unitofWork, ILogger<StateService> logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ExecutedResult<List<LgaResponseDto>>> Lgas(long stateId)
        {
            try
            {
                if(stateId < 0) return ExecutedResult<List<LgaResponseDto>>.BadRequest("Invalid request");
                var result = await _unitofWork.Repository<LGA>().FindAllAsync(s => s.StateId == stateId);                
                if (result.Any())
                {
                    var lgaResponseDto = result.Select(l => new LgaResponseDto
                    {
                        Id = l.Id,
                        Name = l.Name
                    }).ToList();

                    return ExecutedResult<List<LgaResponseDto>>.Success(lgaResponseDto, "Request successful");
                }
                return ExecutedResult<List<LgaResponseDto>>.NotFound("Request successful. No record found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LgaService: An error occured when getting all lga associated with a state");
                return ExecutedResult<List<LgaResponseDto>>.Exception("Something went wrong");
            }
        }
    }
}
