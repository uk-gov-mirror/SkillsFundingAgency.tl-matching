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
    public class When_Provider_Controller_SearchProvider_Is_Loaded_With_Authorised_Admin_User_In_List
    {
        private readonly IActionResult _result;

        public When_Provider_Controller_SearchProvider_Is_Loaded_With_Authorised_Admin_User_In_List()
        {
            var providerService = Substitute.For<IProviderService>();

            var providerController = new ProviderController(providerService, 
                new MatchingConfiguration
                {
                    AuthorisedAdminUserEmail = "user1@test.com;user2@test.com;user3@test.com;"
                });

            var controllerWithClaims = new ClaimsBuilder<ProviderController>(providerController)
                .AddAdminUser()
                .AddUserName("user2")
                .AddEmail("user2@test.com")
                .Build();

            _result = controllerWithClaims.SearchProviderAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ViewModel_Fields_Are_As_Expected()
        {
            var viewResult = _result as ViewResult;
            var viewModel = viewResult?.Model as ProviderSearchViewModel;
            viewModel.Should().NotBeNull();

            viewModel?.IsAuthorisedUser.Should().BeTrue();
        }
    }
}