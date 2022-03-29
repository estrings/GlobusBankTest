using Customer.API.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customer.API.Data.Entities
{
    public class UserOTP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

        public string OTP { get; set; }

        public OTPStatus Status { get; set; }

        public DateTime Created { get; set; }

        public bool IsDeleted { get; set; }

    }
}
