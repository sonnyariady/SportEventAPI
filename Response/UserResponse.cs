using SportEventAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.Response
{
    
        public class UserResponse
        {
            public long Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }

        public UserResponse()
        {

        }

        public UserResponse(User entity)
        {
            this.Id = entity.Id;
            this.FirstName = entity.FirstName;
            this.LastName = entity.LastName;
            this.Email = entity.Email;
                
        }

    }


    public class LoginResult
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
    }
}
