using System;
using System.Collections.Generic;

#nullable disable

namespace SportEventAPI.Models
{
    public partial class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
    }
}
