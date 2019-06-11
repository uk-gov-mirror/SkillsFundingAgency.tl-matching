﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Proximity
{
    public class When_Proximity_Controller_RefineSearchResults_Is_Called_With_Unformated_PostCode
    {
        private readonly IActionResult _result;

        public When_Proximity_Controller_RefineSearchResults_Is_Called_With_Unformated_PostCode()
        {
            const string requestPostcode = "Cv 12 Wt";
            var httpClient = new PostcodesIoHttpClient().Get(requestPostcode);

            var routes = new List<Route>
            {
                new Route {Id = 1, Name = "Route 1"}
            }.AsQueryable();

            var mapper = Substitute.For<IMapper>();

            var proximityService = new ProximityService(Substitute.For<ISearchProvider>(), new LocationApiClient(httpClient, new MatchingConfiguration { PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes" }));

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var opportunityService = Substitute.For<IOpportunityService>();
            var proximityController = new ProximityController(mapper, routePathService, proximityService, opportunityService);

            var viewModel = new SearchParametersViewModel
            {
                Postcode = requestPostcode,
                SelectedRouteId = 1,
                SearchRadius = 10
            };

            _result = proximityController.RefineSearchResults(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null()
        {
            _result.Should().NotBeNull();
        }

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned()
        {
            _result.Should().BeAssignableTo<RedirectToRouteResult>();
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var result = _result as RedirectToRouteResult;
            // ReSharper disable once PossibleNullReferenceException
            result.RouteValues.Count.Should().BeGreaterOrEqualTo(3);
        }

        [Fact]
        public void Then_Result_PostCode_Is_Correctly_Formated()
        {
            var result = _result as RedirectToRouteResult;
            // ReSharper disable once PossibleNullReferenceException
            result.RouteValues["Postcode"].Should().Be("CV1 2WT");
        }
    }
}