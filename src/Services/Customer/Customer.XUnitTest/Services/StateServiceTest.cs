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
    public class StateServiceTest : IStateService
    {
        private readonly List<State> _dbStates;

        public StateServiceTest()
        {
            _dbStates = new List<State>()
            {
                new State() {Id = 1, Name = "Lagos", Lga = null},
                new State() {Id = 2, Name = "Delta", Lga = null},
            };
        }

        public async Task<ExecutedResult<StateResponseDto>> Create(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(ExecutedResult<StateResponseDto>.BadRequest("Invalid request"));

                int count = _dbStates.Count;
                var requestDto = new State
                {
                    Id = count + 1,
                    Name = name
                };

                _dbStates.Add(requestDto);
                var response = new StateResponseDto
                {
                    Id = requestDto.Id,
                    Name = requestDto.Name
                };
                return ExecutedResult<StateResponseDto>.Success(response, "Request successful");
            }
            catch (Exception)
            {
                return ExecutedResult<StateResponseDto>.Exception("Something went wrong");
            }
        }

        public async Task<ExecutedResult<List<StateResponseDto>>> States()
        {
            try
            {
                var result = _dbStates;
                if (result.Any())
                {
                    var stateResponseDto = result.Select(l => new StateResponseDto
                    {
                        Id = l.Id,
                        Name = l.Name
                    }).ToList();
                    return await Task.FromResult(ExecutedResult<List<StateResponseDto>>.Success(stateResponseDto, "Request succesful"));
                }

                return await Task.FromResult(ExecutedResult<List<StateResponseDto>>.Success(null, "Request successful"));
            }
            catch (Exception)
            {
                return ExecutedResult<List<StateResponseDto>>.Exception("Something went wrong");
            }         
        }

        public async Task<ExecutedResult<StateResponseDto>> GetState(long id)
        {
            try
            {
                if (id < 0) return await Task.FromResult(ExecutedResult<StateResponseDto>.BadRequest("Invalid request"));

                var result = _dbStates.FirstOrDefault(s => s.Id == id);
                if (result != null)
                {
                    var response = new StateResponseDto
                    {
                        Id = result.Id,
                        Name = result.Name
                    };
                    return await Task.FromResult(ExecutedResult<StateResponseDto>.Success(response, "Request successful"));
                }
                return await Task.FromResult(ExecutedResult<StateResponseDto>.NotFound("Request successful"));
            }
            catch (Exception)
            {
                return ExecutedResult<StateResponseDto>.Exception("Something went wrong");
            }
        }
    }
}
