using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.DTOs
{
    public class GlobalOutputDto
    {
        public string message { get; set; }
        public int status_code { get; set; }
    }

    public class LoginResultGlobalOutputDto : GlobalOutputDto
    {
        public LoginResultDto data { get; set; }
    }

    public class UserGlobalOutputDto : GlobalOutputDto
    {
        public UserValidationItemDto errors { get; set; }
        public UserResponseDto data { get; set; }

        public UserGlobalOutputDto()
        {
            this.errors = new UserValidationItemDto();
            this.errors.FirstName = new List<string>();
            this.errors.LastName = new List<string>();
            this.errors.Email = new List<string>();
            this.errors.Password = new List<string>();
            this.errors.RepeatPassword = new List<string>();
        }
    }

    public class OrganizerGlobalOutputDto : GlobalOutputDto
    {
        public OrganizerValidationItemDto errors { get; set; }

        public OrganizerDto data { get; set; }
    }

    public class EventGlobalOutputDto : GlobalOutputDto
    {
        public EventValidationItemDto errors { get; set; }

        public EventDto data { get; set; }
    }

    public class UserValidationItemDto
    {
        public List<string> FirstName { get; set; }
        public List<string> LastName { get; set; }
        public List<string> Email { get; set; }
        public List<string> Password { get; set; }
        public List<string> RepeatPassword { get; set; }
    }

    public class OrganizerValidationItemDto
    {
        public List<string> OrganizerName { get; set; }
        public List<string> ImageLocation { get; set; }
    }

    public class EventValidationItemDto
    {
        public List<string> EventDate { get; set; }
        public List<string> EventType { get; set; }
        public List<string> EventName { get; set; }
        public List<string> OrganizerId { get; set; }
    }
}
