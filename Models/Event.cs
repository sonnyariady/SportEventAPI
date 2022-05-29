using System;
using System.Collections.Generic;

#nullable disable

namespace SportEventAPI.Models
{
    public partial class Event
    {
        public long Id { get; set; }
        public string EventDate { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public long OrganizerId { get; set; }
    }
}
