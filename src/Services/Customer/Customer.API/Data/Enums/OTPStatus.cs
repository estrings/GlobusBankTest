using System.ComponentModel;

namespace Customer.API.Data.Enums
{
    public enum OTPStatus
    {
        [Description("new")]
        UNUSED = 0,
        [Description("used")]
        USED = 1,
        [Description("expired")]
        EXPIRED = 2
    }
}
