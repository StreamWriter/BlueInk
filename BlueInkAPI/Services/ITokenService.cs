using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueInk.API.Services
{
    public interface ITokenService
    {
        string BuildToken(string email, string role = "User");
    }
}
