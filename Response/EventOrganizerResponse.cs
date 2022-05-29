using SportEventAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.Response
{
    public class EventOrganizerResponse
    {
        public long Id { get; set; }
        public string EventDate { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public Organizer organizer { get; set; }

        public EventOrganizerResponse()
        {

        }

        public EventOrganizerResponse(Event parent, Organizer organizer )
        {
            this.Id = parent.Id;
            this.EventDate = parent.EventDate;
            this.EventName = parent.EventName;
            this.EventType = parent.EventType;
            this.organizer = organizer;

        }

    }
}
