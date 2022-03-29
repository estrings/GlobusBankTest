using Customer.API.Common.Communication;
using System.Threading.Tasks;

namespace Customer.API.Service.Interfaces
{
    public interface ISMSService
    {
        Task<ExecutedResult<string>> SendSMSAsync(string phoneNumber, string message);
    }
}
