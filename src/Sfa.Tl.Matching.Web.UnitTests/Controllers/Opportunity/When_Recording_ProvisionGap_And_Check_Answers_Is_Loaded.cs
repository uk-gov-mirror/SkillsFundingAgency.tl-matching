﻿using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Recording_ProvisionGap_And_Check_Answers_Is_Loaded
    {
        private readonly IOpportunityService _opportunityService;
        private const string CreatedBy = "CreatedBy";
        private readonly IActionResult _result;

        private const int OpportunityId = 1;

        public When_Recording_ProvisionGap_And_Check_Answers_Is_Loaded()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(CheckAnswersDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var dto = new ValidCheckAnswersDtoBuilder().Build();
            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetCheckAnswers(OpportunityId).Returns(dto);

            var referralService = Substitute.For<IReferralService>();

            var opportunityController = new OpportunityController(_opportunityService, referralService, mapper);
            var controllerWithClaims = new ClaimsBuilder<OpportunityController>(opportunityController)
                .AddUserName(CreatedBy)
                .Build();

            _result = controllerWithClaims.CheckAnswersProvisionGap(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetCheckAnswers_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetCheckAnswers(OpportunityId);
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_OpportunityId_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
        }

        [Fact]
        public void Then_EmployerName_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.EmployerName.Should().Be("EmployerName");
        }

        [Fact]
        public void Then_EmployerContact_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.EmployerContact.Should().Be("EmployerContact");
        }

        [Fact]
        public void Then_SearchRadius_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.SearchRadius.Should().Be(3);
        }

        [Fact]
        public void Then_JobTitle_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.JobTitle.Should().Be("JobTitle");
        }

        [Fact]
        public void Then_PlacementsKnown_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.PlacementsKnown.Should().BeTrue();
        }

        [Fact]
        public void Then_Placements_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.Placements.Should().Be(2);
        }

        [Fact]
        public void Then_Postcode_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.Postcode.Should().Be("AA1 1AA");
        }

        [Fact]
        public void Then_Route_Is_Set()
        {
            var viewModel = _result.GetViewModel<CheckAnswersProvisionGapViewModel>();
            viewModel.PlacementInformation.RouteName.Should().Be("RouteName");
        }
    }
}