using Customer.API.Common.Extensions;

namespace Customer.API.Service.Dtos.Response
{
    public class CustomersResponseDto
    {
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string state { get; set; }
        public string lga { get; set; }
        public bool isAccountActive { get; set; }
    }

    public class ExtendedCustomerResponseDto
    {
        public PageList<CustomersResponseDto> items { get; set; }
        public int TotalCount { get; set; }
    }
}
