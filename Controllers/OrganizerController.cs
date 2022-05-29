using AutoMapper.Internal.Mappers;
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
    public class OrganizerController : ControllerBase
    {
        private readonly IOrganizerServices _organizerServices;
        public OrganizerController(IOrganizerServices organizerServices)
        {
            _organizerServices = organizerServices;
        }

        [MapToApiVersion("1.0")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(OrganizerRequest input)
        {
            var result = await _organizerServices.Create(input);
            if (result.data != null)
            {
                var mapp = ObjectMapper.Mapper.Map<OrganizerDto>(result.data);
                return Ok(mapp);
            }
            else
            {
                var mapNotOK = ObjectMapper.Mapper.Map<OrganizerGlobalOutputDto>(result.data);
                return BadRequest(mapNotOK);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long id,OrganizerRequest input)
        {
            var result = await _organizerServices.Edit(id, input);
            if (result.data != null)
            {
                var mapp = ObjectMapper.Mapper.Map<OrganizerDto>(result.data);
                return Ok(mapp);
            }
            else
            {
                var mapNotOK = ObjectMapper.Mapper.Map<OrganizerGlobalOutputDto>(result.data);
                return BadRequest(mapNotOK);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(long id)
        {
            var result = await _organizerServices.GetById(id);
            if (result != null)
            {
                var mapp = ObjectMapper.Mapper.Map<OrganizerDto>(result);
                return Ok(mapp);
            }
            else
            {
                return NotFound();
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{perPage}/{page}")]

        public async Task<IActionResult> GetAll(int perPage, int page)
        {
            var result = await _organizerServices.GetAll(page, perPage);
            var mapp = ObjectMapper.Mapper.Map<OrganizerDto>(result);
            return Ok(mapp);
       
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _organizerServices.Delete(id);

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
