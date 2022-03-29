using System.ComponentModel.DataAnnotations;

namespace Customer.API.Service.Dtos.Request
{
    public class CustomerRequestDto
    {
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public long state { get; set; }
        [Required]
        public long lga { get; set; }
    }
}
