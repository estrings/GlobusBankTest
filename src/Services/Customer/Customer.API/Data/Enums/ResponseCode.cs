using System.ComponentModel;

namespace Customer.API.Data.Enums
{
    public enum ResponseCode
    {
        [Description("Success")]
        Ok = 00,
        [Description("Not Found")]
        NotFound = 01,
        [Description("Bad Request")]
        ProcessingError = 02,
        [Description("Unauthorized Access")]
        AuthorizationError = 03,
        [Description("Exception Occurred")]
        Exception = 04,
        [Description("Internal Server Error")]
        InternalServer = 05,
        [Description("Not Completed")]
        NotCompleted = 06,
    }
}
