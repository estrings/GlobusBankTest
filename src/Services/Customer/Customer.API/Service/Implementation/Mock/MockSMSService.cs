using Customer.API.Common.Communication;
using Customer.API.Service.Interfaces;
using System.Threading.Tasks;

namespace Customer.API.Service.Implementation.Mock
{
    public class MockSMSService : ISMSService
    {
        public async Task<ExecutedResult<string>> SendSMSAsync(string phoneNumber, string message)
        {
            return await Task.FromResult(ExecutedResult<string>.Success("mock", "Request successful"));
        }
    }
}
