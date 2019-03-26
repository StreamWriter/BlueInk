using System;
using System.Collections.Generic;
using System.Text;

namespace BlueInk.Shared
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
    }

    public struct UserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
