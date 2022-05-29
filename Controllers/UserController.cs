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
    //[Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> Create(CreateUserRequest input)
        {
            var result = await _userServices.Create(input);

            if (result.data != null)
            {
                var mapp = ObjectMapper.Mapper.Map<UserResponseDto>(result.data);
                return Ok(mapp);
            }
            else
            {
                var mapNotOK = ObjectMapper.Mapper.Map<UserGlobalOutputDto>(result);
                return BadRequest(mapNotOK);
            }

        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest input)
        {
            var result = await _userServices.Login(input);

            if (result.data != null)
            {
                var mapp = ObjectMapper.Mapper.Map<LoginResultDto>(result.data);
                return Ok(mapp);
            }
            else
            {
                var mapNotOK = ObjectMapper.Mapper.Map<LoginResultGlobalOutputDto>(result);
                return BadRequest(mapNotOK);
            }

        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id}/Password")]
        public async Task<IActionResult> ChangePassword(long id, ChangePasswordRequest input)
        {
            var result = await _userServices.ChangePassword(id, input);

            if (result.data != null)
            {
                var mapp = ObjectMapper.Mapper.Map<LoginResultDto>(result.data);
                return Ok(mapp);
            }
            else
            {
                var mapNotOK = ObjectMapper.Mapper.Map<LoginResultGlobalOutputDto>(result);
                return BadRequest(mapNotOK);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(long id, BasicUserRequest input)
        {
            var result = await _userServices.Edit(id, input);
            if (result.data != null)
            {
                var mapp = ObjectMapper.Mapper.Map<UserResponseDto>(result.data);
                return Ok(mapp);
            }
            else
            {
                var mapNotOK = ObjectMapper.Mapper.Map<UserGlobalOutputDto>(result);
                return BadRequest(mapNotOK);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _userServices.Delete(id);

            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(new { message = "Error deleting.." });
            }

        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(long id)
        {
            var result = await _userServices.GetById(id);
            if (result != null)
            {
                var mapp = ObjectMapper.Mapper.Map<UserResponseDto>(result);
                return Ok(mapp);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
