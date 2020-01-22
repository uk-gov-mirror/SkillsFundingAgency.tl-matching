﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Recording_Referrals_And_Employer_Details_Is_Submitted_Successfully : IClassFixture<EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel>>
    {
        private readonly EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> _fixture;

        private readonly IActionResult _result;

        public When_Recording_Referrals_And_Employer_Details_Is_Submitted_Successfully(EmployerControllerFixture<EmployerDetailDto, EmployerDetailsViewModel> fixture)
        {
            _fixture = fixture;

            var viewModel = new EmployerDetailsViewModel
            {
                OpportunityItemId = _fixture.OpportunityItemId,
                OpportunityId = _fixture.OpportunityId,
                PrimaryContact = _fixture.EmployerContact,
                Email = _fixture.EmployerContactEmail,
                Phone = _fixture.EmployerContactPhone
            };

            _fixture.OpportunityService.IsReferralOpportunityItemAsync(_fixture.OpportunityItemId).Returns(true);

            var controllerWithClaims = _fixture.Sut.ControllerWithClaims(_fixture.ModifiedBy);

            _fixture.HttpcontextAccesor.HttpContext.Returns(controllerWithClaims.HttpContext);

            _result = controllerWithClaims.SaveOpportunityEmployerDetailsAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GetOpportunity_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService.Received(2).IsReferralOpportunityItemAsync(_fixture.OpportunityItemId);
        }

        [Fact]
        public void Then_SaveEmployerDetail_Is_Called_Exactly_Once()
        {
            _fixture.OpportunityService.Received(3).UpdateOpportunityAsync(Arg.Is<EmployerDetailDto>(a => 
                a.PrimaryContact == _fixture.EmployerContact && 
                a.Email == _fixture.EmployerContactEmail &&
                a.Phone == _fixture.EmployerContactPhone &&
                a.ModifiedBy == _fixture.ModifiedBy));
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetCheckAnswers");
            redirect?.RouteValues["opportunityItemId"].Should().Be(_fixture.OpportunityItemId);
        }
    }
}