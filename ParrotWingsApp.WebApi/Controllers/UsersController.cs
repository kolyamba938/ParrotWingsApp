using Microsoft.AspNetCore.Mvc;
using ParrotWingsApp.Data.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.IdentityModel.Tokens;
using ParrotWingsApp.Data.Model;
using ParrotWingsApp.WebApi.Helpers;
using ParrotWingsApp.WebApi.Model;

namespace ParrotWingsApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        public UsersController(UsersService userService)
        {
            _usersService = userService;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<UserDto> Get()
        {
            var userId = HttpContext.User.Identity.Name;
            var users = _usersService.GetUsersList().ToList();
            users.RemoveAll(u => u.Id == Guid.Parse(userId));
            return users;
        }

        [HttpPost("register")]
        public object Register(UserCredentials credentials)
        {
            try
            {
                _usersService.AddUser(credentials.Email, credentials.Password, credentials.UserName);
                return LogIn(credentials);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorText = e.Message });
            }
        }

        [HttpPost("login")]
        public object LogIn(UserCredentials credentials)
        {
            ClaimsIdentity identity = null;
            try
            {
                identity = GetIdentity(credentials.Email, credentials.Password);
            }
            catch (Exception e)
            {
                return BadRequest(new { errorText = e.Message });
            }
            
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            return response;
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            var user = _usersService.GetUser(email, password);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "")
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        [Authorize]
        [HttpGet("getuserdata")]
        public Object GetUserData()
        {
            var userId = HttpContext.User.Identity.Name;
            return _usersService.GetUser(Guid.Parse(userId));
        }

    }
}
