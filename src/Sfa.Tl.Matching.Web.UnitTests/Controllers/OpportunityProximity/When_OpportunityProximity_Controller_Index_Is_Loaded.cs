using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_Index_Is_Loaded
    {
        private readonly IActionResult _result;
        private readonly IOpportunityService _opportunityService;

        public When_OpportunityProximity_Controller_Index_Is_Loaded()
        {
            var routes = new List<SelectListItem>
            {
                new SelectListItem {Text = "1", Value = "Route 1"},
                new SelectListItem {Text = "2", Value = "Route 2"}
            };

            var routeDictionary = new Dictionary<int, string>
            {
                {1, "Route 1" },
                {2, "Route 2" }
            };

            var proximityopportunityService = Substitute.For<IOpportunityProximityService>();
            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRouteSelectListItemsAsync().Returns(routes);
            routePathService.GetRouteDictionaryAsync().Returns(routeDictionary);

            var locationService = Substitute.For<ILocationService>();

            _opportunityService = Substitute.For<IOpportunityService>();

            var opportunityProximityController = new OpportunityProximityController(routePathService, proximityopportunityService,
                _opportunityService, locationService);

            _result = opportunityProximityController.Index().GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_OpportunityService_GetCompanyNameWithAkaAsync_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceiveWithAnyArgs()
                .GetCompanyNameWithAkaAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ViewModel_CompanyNameWithAka_Should_Not_Be_Set()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<SearchParametersViewModel>();
            viewModel.CompanyNameWithAka.Should().BeNull();
        }
    }
}