using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Core.Helper
{
    public static class OtpHelper
    {
        public static string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
