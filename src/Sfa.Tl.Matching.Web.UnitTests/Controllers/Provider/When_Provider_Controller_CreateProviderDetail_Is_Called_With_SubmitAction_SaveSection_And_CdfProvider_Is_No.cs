using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_CreateProviderDetail_Is_Called_With_SubmitAction_SaveSection_And_CdfProvider_Is_No
    {
        private readonly IActionResult _result;
        private readonly CreateProviderDetailViewModel _viewModel = new CreateProviderDetailViewModel
        {
            SubmitAction = "SaveSection",
            IsCdfProvider = false
        };

        public When_Provider_Controller_CreateProviderDetail_Is_Called_With_SubmitAction_SaveSection_And_CdfProvider_Is_No()
        {
            var providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(providerService, new MatchingConfiguration());
            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController).Build();

            _result = controllerWithClaims.CreateProviderDetail(_viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_SearchProvider()
        {
            var result = _result as RedirectToRouteResult;
            result.Should().NotBeNull();

            result?.RouteName.Should().Be("SearchProvider");
        }
    }
}