using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlueInk.Shared
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public string Role { get; set; }
    }

    public class UserCredentials
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
