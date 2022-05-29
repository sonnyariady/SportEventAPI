using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.DTOs
{
    public class EventDto
    {
        public long Id { get; set; }
        public string EventDate { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public long OrganizerId { get; set; }
    }
}
