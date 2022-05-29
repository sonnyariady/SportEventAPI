using SportEventAPI.Response;
using SportEventAPI.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportEventAPI.Models;

namespace SportEventAPI.Interface
{
    public interface IEventServices
    {
        Task<EventGlobalOutput> Create(EventRequest input);
        Task<List<EventOrganizerResponse>> GetAll(long id, int pagenumber, int pagesize);
        Task<EventOrganizerResponse> GetById(long id);
        Task<bool> Delete(long id);
        Task<EventGlobalOutput> Edit(long id, EventRequest input);
    }
}
