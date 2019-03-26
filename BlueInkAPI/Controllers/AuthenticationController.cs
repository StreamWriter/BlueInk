using BlueInk.API.Data;
using BlueInk.API.Services;
using BlueInk.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlueInk.API.Controllers
{
    [Route("/auth/")]
    public class AuthenticationController : ControllerBase
    {
        private readonly BlueInkDbContext _context;
        private readonly HashingService _hashingService;
        private readonly AuthenticationSettings _authenticationSettings;

        public AuthenticationController(BlueInkDbContext context,
                                        HashingService hashingService,
                                        IOptions<AuthenticationSettings> authenticationSettings)
        {
            _context = context;
            _hashingService = hashingService;
            _authenticationSettings = authenticationSettings.Value;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]UserCredentials credentials)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == credentials.Email);

            if (user != null)
            {
                var hashedPassword = _hashingService.Hash(credentials.Password, user.Salt);

                if (hashedPassword == user.HashedPassword)
                {
                    return Ok(BuildToken(user.Email));
                }
            }

            return BadRequest("Could not login.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserCredentials credentials)
        {
            var alreadyExists = _context.Users.Any(u => u.Email == credentials.Email);

            if (alreadyExists)
            {
                return BadRequest("User already exists.");
            }

            (string hashedPass, string salt) = _hashingService.Hash(credentials.Password);

            var user = new User() { Email = credentials.Email, HashedPassword = hashedPass, Salt = salt };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(BuildToken(user.Email));
        }

        private string BuildToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, email)
               }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(jwtToken);

            return token;
        }

    }
}
