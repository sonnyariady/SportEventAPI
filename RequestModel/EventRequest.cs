using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.RequestModel
{
    public class EventRequest
    {
        public string EventDate { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public long OrganizerId { get; set; }
    }
}
