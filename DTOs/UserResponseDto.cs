using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.DTOs
{
    public class UserResponseDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public UserResponseDto()
        {

        }

   

    }


    public class LoginResultDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
    }
}
