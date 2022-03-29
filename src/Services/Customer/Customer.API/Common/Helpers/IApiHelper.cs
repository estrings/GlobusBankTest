using RestSharp;

namespace Customer.API.Common.Helpers
{
    public interface IApiHelper
    {
        RestClient InitializeClient(string uri);
    }
}
