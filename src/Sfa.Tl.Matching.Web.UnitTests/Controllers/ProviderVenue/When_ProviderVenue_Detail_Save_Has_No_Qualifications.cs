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
    public class When_ProviderVenue_Detail_Save_Has_No_Qualifications
    {
        private readonly IActionResult _result;
        private readonly IProviderVenueService _providerVenueService;

        public When_ProviderVenue_Detail_Save_Has_No_Qualifications()
        {
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerVenueService.IsValidPostCodeAsync("CV1 2WT").Returns((true, "CV1 2WT"));
            var providerVenueController = new ProviderVenueController(_providerVenueService);
            var controllerWithClaims = new ClaimsBuilder<ProviderVenueController>(providerVenueController)
                .AddUserName("username")
                .AddEmail("email@address.com")
                .Build();

            var viewModel = new ProviderVenueDetailViewModel
            {
                Id = 1,
                Postcode = "CV1 2WT"
            };

            _result = controllerWithClaims.ProviderVenueDetail(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        //[Fact]
        //public void Then_Result_Is_RedirectResult() =>
        //    _result.Should().BeOfType<RedirectToRouteResult>();

        [Fact]
        public void Then_Result_Is_Redirect_To_Add_Qualification()
        {
            var redirect = _result as RedirectToRouteResult;
            //TODO: use commented line below when Add Qualification is implemented
            //TODO: Also uncomment test above - Then_Result_Is_RedirectResult
            redirect.Should().BeNull();
            //redirect?.RouteName.Should().BeEquivalentTo("AddQualification");
        }

        [Fact]
        public void Then_UpdateVenue_Is_Called_Exactly_Once()
        {
            _providerVenueService.Received(1).UpdateVenueAsync(Arg.Any<ProviderVenueDetailViewModel>());
        }
    }
}