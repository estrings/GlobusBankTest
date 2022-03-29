using Customer.API.Common.Communication;
using Customer.API.Common.ResourceParameters;
using Customer.API.Service.Dtos.Request;
using Customer.API.Service.Dtos.Response;
using System.Threading.Tasks;

namespace Customer.API.Service.Interfaces
{
    public interface ICustomerService
    {
        Task<ExecutedResult<CustomerResponseDto>> CreateCustomer(CustomerRequestDto customerRequestDto);
        Task<ExecutedResult<VerifyCustomerResponseDto>> VerifyCustomer(VerifyCustomerRequestDto verifyCustomerRequestDto);
        Task<ExecutedResult<ExtendedCustomerResponseDto>> Customers(ResourceParameters resourceParameters);
    }
}
