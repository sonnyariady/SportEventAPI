using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.DTOs
{
    public class OrganizerDto
    {
        public long Id { get; set; }
        public string OrganizerName { get; set; }
        public string ImageLocation { get; set; }
    }
}
