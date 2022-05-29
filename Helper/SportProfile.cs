using AutoMapper;
using SportEventAPI.DTOs;
using SportEventAPI.Models;
using SportEventAPI.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportEventAPI.Helper
{
    public class SportProfile : Profile
    {
        public SportProfile()
        {
            AllowNullCollections = true;

            #region Event <=> EventDto
            CreateMap<Event, EventDto>().ReverseMap();
            #endregion

            #region Organizer <=> OrganizerDto
            CreateMap<Organizer, OrganizerDto>().ReverseMap();
            #endregion

            #region UserResponse <=> UserResponseDto
            CreateMap<UserResponse, UserResponseDto>().ReverseMap();
            #endregion

            #region LoginResult <=> LoginResultDto
            CreateMap<LoginResult, LoginResultDto>().ReverseMap();
            #endregion

            #region GlobalOutput <=> GlobalOutputDto
            CreateMap<GlobalOutput, UserResponseDto>().ReverseMap();
            #endregion

            #region LoginResultGlobalOutput <=> LoginResultGlobalOutputDto
            CreateMap<LoginResultGlobalOutput, LoginResultGlobalOutputDto>().ReverseMap();
            #endregion

            #region UserGlobalOutput <=> UserGlobalOutputDto
            CreateMap<UserGlobalOutput, UserGlobalOutputDto>().ReverseMap();
            #endregion

            #region UserValidationItem <=> UserValidationItemDto
            CreateMap<UserValidationItem, UserValidationItemDto>().ReverseMap();
            #endregion

            #region OrganizerGlobalOutput <=> OrganizerGlobalOutputDto
            CreateMap<OrganizerGlobalOutput, OrganizerGlobalOutputDto>().ReverseMap();
            #endregion

            #region OrganizerValidationItem <=> OrganizerValidationItemDto
            CreateMap<OrganizerValidationItem, OrganizerValidationItemDto>().ReverseMap();
            #endregion

            #region EventOrganizerResponse <=> EventOrganizerResponseDto
            CreateMap<EventOrganizerResponse, EventOrganizerResponseDto>().ReverseMap();
            #endregion

            #region EventValidationItem <=> EventValidationItemDto
            CreateMap<EventValidationItem, EventValidationItemDto>().ReverseMap();
            #endregion
        }
    }
}
