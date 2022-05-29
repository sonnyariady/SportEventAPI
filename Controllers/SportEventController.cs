using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportEventAPI.DTOs;
using SportEventAPI.Helper;
using SportEventAPI.Interface;
using SportEventAPI.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
  //  [Route("api/v{version:apiVersion}/[controller]")]
    public class SportEventController : ControllerBase
    {
        private readonly IEventServices _eventServices;
        public SportEventController(IEventServices eventServices)
        {
            _eventServices = eventServices;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(EventRequest input)
        {
            var result = await _eventServices.Create(input);

            if (result.data != null)
            {
                var mapp = ObjectMapper.Mapper.Map<EventDto>(result.data);
                return Ok(mapp);
            }
            else
            {
                var mapNotOk = ObjectMapper.Mapper.Map<EventGlobalOutputDto>(result);
                return BadRequest(mapNotOk);
            }

        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(long id)
        {
            var result = await _eventServices.GetById(id);
            if (result != null)
            {
                var mapp = ObjectMapper.Mapper.Map<EventOrganizerResponseDto>(result);
                return Ok(mapp);
            }
            else
            {
                return NotFound();
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{page}/{perPage}/{organizerId}")]
        public async Task<IActionResult> GetAll(int page, int perPage, long organizerId)
        {
            var result = await _eventServices.GetAll(organizerId, page, perPage);
            var mapp = ObjectMapper.Mapper.Map<List<EventOrganizerResponseDto>>(result);
            return Ok(mapp);

        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _eventServices.Delete(id);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = "Error deleting.." });
            }

        }

    }
}
