using System;
using System.Collections.Generic;

#nullable disable

namespace SportEventAPI.Models
{
    public partial class Organizer
    {
        public long Id { get; set; }
        public string OrganizerName { get; set; }
        public string ImageLocation { get; set; }
    }
}
