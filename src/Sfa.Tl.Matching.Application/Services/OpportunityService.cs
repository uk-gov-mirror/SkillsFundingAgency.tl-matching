﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Opportunity> _opportunityRepository;
        private readonly IRepository<ProvisionGap> _provisionGapRepository;
        private readonly IRepository<Referral> _referralRepository;

        public OpportunityService(
            IMapper mapper,
            IRepository<Opportunity> opportunityRepository,
            IRepository<ProvisionGap> provisionGapRepository,
            IRepository<Referral> referralRepository)
        {
            _mapper = mapper;
            _opportunityRepository = opportunityRepository;
            _provisionGapRepository = provisionGapRepository;
            _referralRepository = referralRepository;
        }

        public async Task<int> CreateOpportunity(OpportunityDto dto)
        {
            var opportunity = _mapper.Map<Opportunity>(dto);

            return await _opportunityRepository.Create(opportunity);
        }

        public async Task<OpportunityDto> GetOpportunity(int id)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == id);

            var dto = _mapper.Map<Opportunity, OpportunityDto>(opportunity);

            return dto;
        }

        public async Task<bool> IsReferralOpportunity(int id)
        {
            return await _opportunityRepository.GetMany(o => o.Id == id && o.Referral.Any()).AnyAsync();
        }

        public async Task<PlacementInformationSaveDto> GetPlacementInformationSave(int id)
        {
            var placementInformation = await _opportunityRepository.GetSingleOrDefault(e => e.Id == id,
                opp => opp.Route);

            var dto = _mapper.Map<Opportunity, PlacementInformationSaveDto>(placementInformation);

            return dto;
        }

        public async Task<CheckAnswersDto> GetCheckAnswers(int id)
        {
            var checkAnswers = await _opportunityRepository.GetSingleOrDefault(e => e.Id == id,
                opp => opp.Route);

            var dto = _mapper.Map<Opportunity, CheckAnswersDto>(checkAnswers);

            return dto;
        }

        public async Task UpdateOpportunity<T>(T dto) where T : BaseOpportunityUpdateDto
        {
            var trackedEntity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == dto.OpportunityId);
            trackedEntity = _mapper.Map(dto, trackedEntity);
            
            await _opportunityRepository.Update(trackedEntity);
        }

        public Task<int> CreateProvisionGap(CheckAnswersProvisionGapViewModel dto)
        {
            var provisionGap = _mapper.Map<ProvisionGap>(dto);

            return _provisionGapRepository.Create(provisionGap);
        }

        public List<ReferralDto> GetReferrals(int opportunityId)
        {
            var referrals = _referralRepository.GetMany(r => r.OpportunityId == opportunityId,
                r => r.ProviderVenue, r => r.ProviderVenue.Provider);

            var providers = referrals
                .OrderBy(r => r.DistanceFromEmployer)
                .Select(r => new ReferralDto
                {
                    Name = r.ProviderVenue.Provider.Name,
                    Postcode = r.ProviderVenue.Postcode,
                    DistanceFromEmployer = r.DistanceFromEmployer
                })
                .ToList();

            return providers;
        }
    }
}