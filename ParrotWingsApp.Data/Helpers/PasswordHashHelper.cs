using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ParrotWingsApp.Data.Helpers
{
    public static class PasswordHashHelper
    {
        public static string GetMd5Hash(String input)
        {
            using var md5 = MD5.Create();
            var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
            return Encoding.ASCII.GetString(result);
        }
    }
}