﻿using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class UserCacheMapper : Profile
    {
        public UserCacheMapper()
        {
            CreateMap<UserCacheDto, UserCache>()
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<UserCacheDto, UserCache>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<UserCacheDto, UserCache>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<UserCacheDto, UserCache>>())
                .ForMember(m => m.UrlHistory, config => config.Ignore());
        }
    }
}