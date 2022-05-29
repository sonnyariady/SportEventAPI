using SportEventAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SportEventAPI.Response.UserResponse;

namespace SportEventAPI.Response
{
    public class GlobalOutput
    {
        public string message { get; set; }
        public int status_code { get; set; }
    }

    public class LoginResultGlobalOutput : GlobalOutput
    {
        public LoginResult data { get; set; }
    }

    public class UserGlobalOutput : GlobalOutput
    {
        public UserValidationItem errors { get; set; }
        public UserResponse data { get; set; }

        public UserGlobalOutput()
        {
            this.errors = new UserValidationItem();
            this.errors.FirstName = new List<string>();
            this.errors.LastName = new List<string>();
            this.errors.Email = new List<string>();
            this.errors.Password = new List<string>();
            this.errors.RepeatPassword = new List<string>();
        }
    }

    public class OrganizerGlobalOutput : GlobalOutput
    {
        public OrganizerValidationItem errors { get; set; }

        public Organizer data { get; set; }
    }

    public class EventGlobalOutput : GlobalOutput
    {
        public EventValidationItem errors { get; set; }

        public Event data { get; set; }
    }

    public class UserValidationItem
    {
        public List<string> FirstName { get; set; }
        public List<string> LastName { get; set; }
        public List<string> Email { get; set; }
        public List<string> Password { get; set; }
        public List<string> RepeatPassword { get; set; }
        public List<string> Id { get; set; }
    }

    public class OrganizerValidationItem
    {
        public List<string> OrganizerName { get; set; }
        public List<string> ImageLocation { get; set; }
    }

    public class EventValidationItem
    {
        public List<string> EventDate { get; set; }
        public List<string> EventType { get; set; }
        public List<string> EventName { get; set; }
        public List<string> OrganizerId { get; set; }
    }

}
