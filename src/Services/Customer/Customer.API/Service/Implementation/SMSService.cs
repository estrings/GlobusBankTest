using Customer.API.Common.Communication;
using Customer.API.Service.Interfaces;
using System;
using System.Threading.Tasks;

namespace Customer.API.Service.Implementation
{
    public class SMSService : ISMSService
    {
        //this will contain actual implementation using an SMS service gateway as provided by the business
        public Task<ExecutedResult<string>> SendSMSAsync(string phoneNumber, string message)
        {
            throw new NotImplementedException();
        }
    }
}
