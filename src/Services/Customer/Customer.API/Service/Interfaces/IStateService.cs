using Customer.API.Common.Communication;
using Customer.API.Service.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customer.API.Service.Interfaces
{
    public interface IStateService
    {
        Task<ExecutedResult<StateResponseDto>> Create(string name);
        Task<ExecutedResult<List<StateResponseDto>>> States();
        Task<ExecutedResult<StateResponseDto>> GetState(long id);
    }
}
