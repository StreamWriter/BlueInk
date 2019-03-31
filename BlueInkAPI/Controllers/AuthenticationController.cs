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
        private readonly ITokenService _tokenService;

        public AuthenticationController(BlueInkDbContext context,
                                        HashingService hashingService,
                                        ITokenService tokenService)
        {
            _context = context;
            _hashingService = hashingService;
            _tokenService = tokenService;
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
                    return Ok(_tokenService.BuildToken(user.Email));
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

            var user = new User() { Email = credentials.Email, HashedPassword = hashedPass, Salt = salt, Role = "User" };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(_tokenService.BuildToken(user.Email, user.Role));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOwner([FromBody]UserCredentials credentials)
        {
            var adminExists = _context.Users.Any(u => u.Role == "Admin");

            if (adminExists)
            {
                return BadRequest("The site owner has already been initialized.");
            }

            (string hashedPass, string salt) = _hashingService.Hash(credentials.Password);

            var user = new User() { Email = credentials.Email, HashedPassword = hashedPass, Salt = salt, Role = "Admin" };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(_tokenService.BuildToken(user.Email, user.Role));
        }

    }
}
