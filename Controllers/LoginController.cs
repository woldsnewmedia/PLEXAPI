using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// JWT
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PLEXAPI.Data;
using PLEXAPI.Models;

namespace PLEXAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        // POST login 
        [AllowAnonymous]
        [HttpPost]
        [Route("/Login")]
        public IActionResult LoginPost([FromBody]JWTLoginModel login)
        {

            IActionResult response = Unauthorized();
            var user = AuthenticateUserPost(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;

        }

        // Create token
        private string GenerateJSONWebToken(JWTLoginModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Authenticate user Post
        private JWTLoginModel AuthenticateUserPost(JWTLoginModel login)
        {
            JWTLoginModel user = null;

            // Validate user against database here - following is test as no user db added yet

            // POST
            if (login.Username == "apitest")
            {
                user = new JWTLoginModel { Username = "apitest", Password = "password123" };
            }


            return user;

        }


    }

}




