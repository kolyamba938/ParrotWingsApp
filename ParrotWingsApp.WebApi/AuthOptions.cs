using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace ParrotWingsApp.WebApi
{
    public class AuthOptions
    {
        public const string ISSUER = "PwAuthServer"; 
        public const string AUDIENCE = "PwAuthClient"; 
        const string KEY = "parrotWings_secretKey@123!";   
        public const int LIFETIME = 10; // 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
