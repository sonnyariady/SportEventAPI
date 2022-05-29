using SportEventAPI.Models;
using SportEventAPI.Response;
using SportEventAPI.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.Interface
{
    public interface IOrganizerServices
    {
        Task<Organizer> GetById(long id);
        Task<OrganizerGlobalOutput> Edit(long id, OrganizerRequest input);
        Task<OrganizerGlobalOutput> Create(OrganizerRequest input);
        Task<List<Organizer>> GetAll(int pagenumber, int pagesize);
        Task<bool> Delete(long id);
    }
}
