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
using PLEXAPI.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace PLEXAPI.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private IConfiguration _config;

        private readonly APIDbContext _context;

        public LoginController(IConfiguration config, APIDbContext context)
        {
            _config = config;
            _context = context;

        }

        // POST Register new user
        [AllowAnonymous]
        [HttpPost]
        [Route("/Register")]
        public async Task<APIUser> RegisterNewAPIUser([FromBody]APIUserRegister a)
        {

            if (await _context.APIUser.AnyAsync(x => x.Username == a.Username))
            {
                return new APIUser() { };
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(a.Password, out passwordHash, out passwordSalt);

            APIUser apiuser = new APIUser() {
                EmailAddress = a.EmailAddress,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Username = a.Username
            };
            await _context.APIUser.AddAsync(apiuser);
            await _context.SaveChangesAsync();

            return apiuser;

        }

        // PW hash
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // POST login 
        [AllowAnonymous]
        [HttpPost]
        [Route("/Login")]
        public async Task<IActionResult> LoginPostAsync([FromBody]JWTLoginModel login)
        {

            IActionResult response = Unauthorized();

            JWTLoginModel user = await AuthenticateUserPostAsync(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;

        }

        // Authenticate user Post
        private async Task<JWTLoginModel> AuthenticateUserPostAsync(JWTLoginModel login)
        {

            var user = await _context.APIUser.FirstOrDefaultAsync(x => x.Username == login.Username);

            if (user == null)
                return null;

            if (!VerifyPassword(login.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return new JWTLoginModel()
            {
                Username = user.Username,
                Password = user.PasswordHash.ToString()
            };

        }

        // PW verify - hash pw passed in and compare
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)   // mismatch check
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        // Create token
        private string GenerateJSONWebToken(JWTLoginModel u)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, u.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }













    }

}




