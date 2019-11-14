﻿using AutoMapper;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmployerMapper : Profile
    {
        public EmployerMapper()
        {
            CreateMap<Employer, EmployerSearchResultDto>();

            CreateMap<EmployerStagingDto, EmployerStaging>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ChecksumCol, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<CrmEmployerEventBase, Employer>()
                .ForMember(m => m.CrmId, config => config.MapFrom(s => s.accountid.ToGuid()))
                .ForMember(m => m.CompanyName, config => config.MapFrom(s => s.Name))
                .ForMember(m => m.AlsoKnownAs, config => config.MapFrom(s => s.sfa_alias))
                .ForMember(m => m.CompanyNameSearch, config => config.MapFrom(s => s.Name.ToLetterOrDigit() + s.sfa_alias.ToLetterOrDigit()))
                .ForMember(m => m.Aupa, config => config.MapFrom(s => s.sfa_aupa.ToAupaStatus()))
                .ForMember(m => m.PrimaryContact, config => config.MapFrom(s => s.PrimaryContactId.name))
                .ForMember(m => m.Phone, config => config.MapFrom(s => s.ContactTelephone1))
                .ForMember(m => m.Email, config => config.MapFrom(s => s.ContactEmail))
                .ForMember(m => m.Owner, config => config.MapFrom(s => s.owneridname))
                .ForAllOtherMembers(c => c.Ignore())
                ;
        }
    }
}