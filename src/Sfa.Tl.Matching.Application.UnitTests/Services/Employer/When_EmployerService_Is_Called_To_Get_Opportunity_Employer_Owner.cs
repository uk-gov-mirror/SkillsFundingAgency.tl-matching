﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_EmployerService_Is_Called_To_Get_Opportunity_Employer_Owner
    {
        private readonly string _result;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_EmployerService_Is_Called_To_Get_Opportunity_Employer_Owner()
        {
            var employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();

            var opportunity = new Domain.Models.Opportunity
            {
                Id = 1,
                CreatedBy = "CreatedBy"
            };

            _opportunityRepository.GetFirstOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>())
                .Returns(opportunity);

            var employerService = new EmployerService(employerRepository, _opportunityRepository, Substitute.For<IMapper>(), Substitute.For<IValidator<EmployerStagingFileImportDto>>());

            _result = employerService.GetEmployerOpportunityOwnerAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetFirstOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetFirstOrDefault(Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>());
        }

        [Fact]
        public void Then_Result_Is_Created_By_User_Name()
        {
            _result.Should().Be("CreatedBy");
        }
    }
}