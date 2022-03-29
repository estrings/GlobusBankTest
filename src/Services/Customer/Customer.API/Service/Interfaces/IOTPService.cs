using Customer.API.Common.Communication;
using System.Threading.Tasks;

namespace Customer.API.Service.Interfaces
{
    public interface IOTPService
    {
        Task<ExecutedResult<string>> SendOTP(string phoneNumber);
        Task<ExecutedResult<string>> ValidateOTP(string phoneNumber, string otp);
    }
}
