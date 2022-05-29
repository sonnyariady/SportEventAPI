using SportEventAPI.Models;
using SportEventAPI.Response;
using Microsoft.EntityFrameworkCore;
using SportEventAPI.Interface;
using SportEventAPI.Helper;
using SportEventAPI.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.Services
{
    public class OrganizerServices : IOrganizerServices
    {
        private readonly SportEventsContext _context;

        public OrganizerServices(SportEventsContext context)
        {
            _context = context;
        }

        public async Task<OrganizerGlobalOutput> Create(OrganizerRequest input)
        {
            OrganizerGlobalOutput globaloutput = new OrganizerGlobalOutput();
            globaloutput.errors = new OrganizerValidationItem();
            bool IsValid = true;
            try
            {
                if (string.IsNullOrWhiteSpace(input.OrganizerName))
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.OrganizerName.Add("The Organizer Name field is required.");
                }
                if (string.IsNullOrWhiteSpace(input.ImageLocation))
                {
                    globaloutput.status_code = 422;
                    IsValid = false;
                    globaloutput.errors.ImageLocation.Add("The ImageLocation field is required.");
                }

                if (IsValid)
                {
                    Organizer organizer = new Organizer();
                    organizer.OrganizerName = input.OrganizerName;
                    organizer.ImageLocation = input.ImageLocation;
                    await _context.Organizers.AddAsync(organizer);
                    await _context.SaveChangesAsync();

                    globaloutput.data = organizer;
                }

            }
            catch (Exception ex)
            {


            }
            return globaloutput;
        }

        public async Task<OrganizerGlobalOutput> Edit(long id, OrganizerRequest input)
        {
            OrganizerGlobalOutput globaloutput = new OrganizerGlobalOutput();
            globaloutput.errors = new OrganizerValidationItem();
            try
            {
                var organizer = await _context.Organizers.FindAsync(id);

                if (organizer == null)
                {

                }


                if (string.IsNullOrWhiteSpace(input.OrganizerName))
                {
                    globaloutput.errors.OrganizerName.Add("The Organizer Name field is required.");
                }
                if (string.IsNullOrWhiteSpace(input.ImageLocation))
                {
                    globaloutput.errors.ImageLocation.Add("The ImageLocation field is required.");
                }


               
                organizer.OrganizerName = input.OrganizerName;
                organizer.ImageLocation = input.ImageLocation;
                _context.Entry(organizer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                globaloutput.data = organizer;

            }
            catch (Exception ex)
            {


            }
            return globaloutput;
        }

        public async Task<Organizer> GetById(long id)
        {
            Organizer oOrganizer = null;
            try
            {
                oOrganizer = await _context.Organizers.FindAsync(id);
            }
            catch (Exception ex)
            {

                 
            }
            return oOrganizer;
        }

        public async Task<bool> Delete(long id)
        {
           
            try
            {
                var oOrganizer = await _context.Organizers.FindAsync(id);

                if (oOrganizer == null)
                {
                    throw new Exception("Organizer Id is not registered.");
                }

                //sampai ke detail
                var lstEvent = await _context.Events.Where(a => a.OrganizerId == id).ToListAsync();

                foreach (var itemchild in lstEvent)
                {
                    _context.Events.Remove(itemchild);
                }
                _context.Organizers.Remove(oOrganizer);
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        public async Task<List<Organizer>> GetAll(int pagenumber, int pagesize)
        {
            return await _context.Organizers.Page(pagenumber, pagesize).ToListAsync();
        }

    }
}
