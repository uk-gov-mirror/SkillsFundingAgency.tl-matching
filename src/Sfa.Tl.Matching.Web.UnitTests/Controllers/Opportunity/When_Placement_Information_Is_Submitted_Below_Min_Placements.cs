﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Opportunity
{
    public class When_Placement_Information_Is_Submitted_Below_Min_Placements
    {
        private readonly IActionResult _result;
        private readonly OpportunityController _opportunityController;

        public When_Placement_Information_Is_Submitted_Below_Min_Placements()
        {
            var opportunityService = Substitute.For<IOpportunityService>();
            var referralService = Substitute.For<IReferralService>();

            var viewModel = new PlacementInformationSaveViewModel
            {
                PlacementsKnown = true,
                Placements = 0
            };

            _opportunityController = new OpportunityController(opportunityService, referralService);

            _result = _opportunityController.PlacementInformationSave(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_State_Has_1_Error() =>
            _opportunityController.ViewData.ModelState.Should().ContainSingle();

        [Fact]
        public void Then_Model_State_Has_Min_Key() =>
            _opportunityController.ViewData.ModelState.ContainsKey(nameof(PlacementInformationSaveViewModel.Placements))
                .Should().BeTrue();

        [Fact]
        public void Then_Model_State_Has_Min_Error()
        {
            var modelStateEntry = _opportunityController.ViewData.ModelState[nameof(PlacementInformationSaveViewModel.Placements)];
            modelStateEntry.Errors[0].ErrorMessage.Should().Be("The number of placements must be 1 or more");
        }
    }
}