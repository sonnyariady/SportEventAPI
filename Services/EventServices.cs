using SportEventAPI.Models;
using SportEventAPI.Response;
using SportEventAPI.Interface;
using SportEventAPI.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportEventAPI.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SportEventAPI.Services
{
    public class EventServices : IEventServices
    {
        private readonly SportEventsContext _context;
        private readonly ILogger<EventServices> _logger;
        public EventServices(SportEventsContext context, ILogger<EventServices> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<EventGlobalOutput> Create(EventRequest input)
        {
            EventGlobalOutput globaloutput = new EventGlobalOutput();
            globaloutput.errors = new EventValidationItem();
            bool IsValid = true;
            try
            {
                if (string.IsNullOrWhiteSpace(input.EventDate))
                {
                    globaloutput.errors.EventDate.Add("The Event Date field is required.");
                    globaloutput.status_code = 422;
                    IsValid = false;
                }
                else
                {

                }
                if (string.IsNullOrWhiteSpace(input.EventName))
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.EventName.Add("The Event Name field is required.");
                }

                if (string.IsNullOrWhiteSpace(input.EventType))
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.EventType.Add("The Event Type field is required.");
                }

                var organizer = await _context.Organizers.FindAsync(input.OrganizerId);

                if (organizer == null)
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.OrganizerId.Add("The Organizer Id is not registered.");
                }

                if (IsValid)
                {
                    Event oevent = new Event();
                    oevent.EventName = input.EventName;
                    oevent.EventType = input.EventType;
                    oevent.EventDate = input.EventDate;
                    oevent.OrganizerId = input.OrganizerId;
                    await _context.Events.AddAsync(oevent);
                    await _context.SaveChangesAsync();

                    globaloutput.data = oevent;
                }
                  
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EventServices.Create");
            }

            return globaloutput;
        }

        public async Task<List<EventOrganizerResponse>> GetAll(long id, int pagenumber, int pagesize)
        {
            List<EventOrganizerResponse> lst = new List<EventOrganizerResponse>();

            var organizer = await _context.Organizers.FindAsync(id);

            var list = await _context.Events.Where(a=> a.OrganizerId == id).Page(pagenumber, pagesize).ToListAsync();

            foreach (var item in list)
            {
                EventOrganizerResponse viewitem = new EventOrganizerResponse(item, organizer);
                lst.Add(viewitem);
            }
            return lst;
        }

        public async Task<EventOrganizerResponse> GetById(long id)
        {
            EventOrganizerResponse eventOrganizer = null;

            var oEvent = await _context.Events.FindAsync(id);

            var organizer = await _context.Organizers.FindAsync(oEvent.OrganizerId);

            eventOrganizer = new EventOrganizerResponse(oEvent, organizer);


            return eventOrganizer;
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                var oEvent = await _context.Events.FindAsync(id);
                if (oEvent == null)
                {
                    throw new Exception("Event is not found.");
                }
                _context.Events.Remove(oEvent);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EventServices.Delete");
                return false;
            }

            return true;
        }

        public async Task<EventGlobalOutput> Edit(long id, EventRequest input)
        {
            EventGlobalOutput globaloutput = new EventGlobalOutput();
            bool IsValid = true;
            try
            {
                var oEvent = await _context.Events.FindAsync(id);
                if (string.IsNullOrWhiteSpace(input.EventDate))
                {
                    globaloutput.errors.EventDate.Add("The Event Date field is required.");
                    globaloutput.status_code = 422;
                    IsValid = false;
                }
                else
                {

                }
                if (string.IsNullOrWhiteSpace(input.EventName))
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.EventName.Add("The Event Name field is required.");
                }

                if (string.IsNullOrWhiteSpace(input.EventType))
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.EventType.Add("The Event Type field is required.");
                }

                var organizer = await _context.Organizers.FindAsync(input.OrganizerId);

                if (organizer == null)
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.OrganizerId.Add("The Organizer Id is not registered.");
                }

                if (IsValid)
                {
                    oEvent.EventDate = input.EventDate;
                    oEvent.EventName = input.EventName;
                    oEvent.EventType = input.EventType;
                    _context.Entry(oEvent).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    
                    globaloutput.data = oEvent;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EventServices.Edit");
                
            }
            return globaloutput;
        }
    }
}
