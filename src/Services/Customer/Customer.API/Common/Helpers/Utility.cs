using System;
using System.Security.Cryptography;

namespace Customer.API.Common.Helpers
{
    public static class Utility
    {
        public static string GenerateOTP()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[8];
            provider.GetBytes(byteArray);

            var num = BitConverter.ToUInt64(byteArray, 0);
            return num.ToString().Substring(0, 6);
        }

        public static string MaskPhoneNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number)) return string.Empty;
            return number.Replace(number.Substring(3, 5), "**");
        }
    }
}
