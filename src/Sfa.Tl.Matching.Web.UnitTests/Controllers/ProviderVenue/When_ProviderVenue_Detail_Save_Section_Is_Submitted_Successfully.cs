using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderVenue
{
    public class When_ProviderVenue_Detail_Save_Section_Is_Submitted_Successfully
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Detail_Save_Section_Is_Submitted_Successfully()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostcodeAsync("CV1 2WT").Returns((true, "CV1 2WT"));

            var providerVenueController = new ProviderVenueController(_providerVenueService);
            var controllerWithClaims = new ClaimsBuilder<ProviderVenueController>(providerVenueController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new ProviderVenueDetailViewModel
            {
                Id = 1,
                ProviderId = 2,
                Postcode = "CV1 2WT",
                SubmitAction = "SaveSection"
            };

            _result = controllerWithClaims.SaveProviderVenueDetail(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Result_Is_Redirect_To_Get_Provider_Venue_Detail()
        {
            var redirect = _result as RedirectToRouteResult;
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderVenueDetail");

            redirect?.RouteValues["providerVenueId"].Should().Be(1);
            redirect?.RouteValues["providerId"].Should().Be(2);
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_GetVenueWithQualifications_Is_Not_Called()
        {
            _providerVenueService.DidNotReceive().GetVenueWithQualificationsAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_UpdateVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).UpdateVenueAsync(Arg.Any<ProviderVenueDetailViewModel>());
        }
    }
}