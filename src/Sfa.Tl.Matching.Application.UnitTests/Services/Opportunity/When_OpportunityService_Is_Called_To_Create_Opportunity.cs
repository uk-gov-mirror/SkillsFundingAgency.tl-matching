﻿using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Opportunity
{
    public class When_OpportunityService_Is_Called_To_Create_Opportunity
    {
        private readonly int _result;
        private const int OpportunityId = 1;

        private readonly IRepository<Domain.Models.Opportunity> _opportunityRepository;

        public When_OpportunityService_Is_Called_To_Create_Opportunity()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(OpportunityMapper).Assembly));
            var mapper = new Mapper(config);
            
            _opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();
            var opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();
            var provisionGapRepository = Substitute.For<IRepository<ProvisionGap>>();
            var referralRepository = Substitute.For<IRepository<Domain.Models.Referral>>();

            _opportunityRepository.Create(Arg.Any<Domain.Models.Opportunity>())
                .Returns(OpportunityId);

            var opportunityService = new OpportunityService(mapper, _opportunityRepository, opportunityItemRepository, provisionGapRepository, referralRepository);

            var dto = new OpportunityDto
            {
                EmployerContact = "EmployerContact"
            };

            _result = opportunityService.CreateOpportunity(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityItemRepository_Create_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.Opportunity>(opportunity => 
                    opportunity.EmployerContact == "EmployerContact"
            ));
        }

        [Fact]
        public void Then_OpportunityId_Is_Created()
        {
            _result.Should().Be(OpportunityId);
        }
    }
}