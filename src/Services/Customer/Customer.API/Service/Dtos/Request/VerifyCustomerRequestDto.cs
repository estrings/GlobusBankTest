using System.ComponentModel.DataAnnotations;

namespace Customer.API.Service.Dtos.Request
{
    public class VerifyCustomerRequestDto
    {
        [Required]
        public long customerId { get; set; }
        [Required]
        public string otp { get; set; }
        [Required]
        public string phoneNumber { get; set; }
    }
}
