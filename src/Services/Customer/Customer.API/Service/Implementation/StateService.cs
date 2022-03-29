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
    public class StateService : IStateService
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ILogger<StateService> _logger;

        public StateService(IUnitofWork unitofWork, ILogger<StateService> logger)
        {
            _unitofWork = unitofWork;
            _logger = logger;
        }

        public async Task<ExecutedResult<StateResponseDto>> Create(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name)) return ExecutedResult<StateResponseDto>.BadRequest("State name is required");

                var requestDto = new State
                {
                    Name = name
                };

                var result = await _unitofWork.Repository<State>().AddAsync(requestDto);
                var response = new StateResponseDto
                {
                    Id = result.Id,
                    Name = result.Name
                };
                return ExecutedResult<StateResponseDto>.Success(response, "Request successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StateService: An error occured when creating state");
                return ExecutedResult<StateResponseDto>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<StateResponseDto>> GetState(long id)
        {
            try
            {
                if (id < 0) return ExecutedResult<StateResponseDto>.BadRequest("Invalid request");

                var result = await _unitofWork.Repository<State>().FindAsync(s => s.Id == id);
                if(result != null)
                {
                    var response = new StateResponseDto
                    {
                        Id = result.Id,
                        Name = result.Name
                    };
                    return ExecutedResult<StateResponseDto>.Success(response, "Request successful");
                }
                return ExecutedResult<StateResponseDto>.NotFound("No record found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StateService: An error occured when getting state");
                return ExecutedResult<StateResponseDto>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<List<StateResponseDto>>> States()
        {
            try
            {
                var result = await _unitofWork.Repository<State>().GetAllAsync();
                if (result.Any())
                {
                    var stateResponseDto = result.Select(l => new StateResponseDto
                    {
                        Id = l.Id,
                        Name = l.Name
                    }).ToList();
                    return ExecutedResult<List<StateResponseDto>>.Success(stateResponseDto, "Request successful");
                }
                return ExecutedResult<List<StateResponseDto>>.Success(null, "Request successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "StateService: An error occured when getting all state");
                return ExecutedResult<List<StateResponseDto>>.Exception("Something went wrong");
            }
        }
    }
}
