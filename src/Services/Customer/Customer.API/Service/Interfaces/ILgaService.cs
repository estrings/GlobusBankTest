﻿using Customer.API.Common.Communication;
using Customer.API.Service.Dtos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customer.API.Service.Interfaces
{
    public interface ILgaService
    {
        Task<ExecutedResult<List<LgaResponseDto>>> Lgas(long stateId);
    }
}
