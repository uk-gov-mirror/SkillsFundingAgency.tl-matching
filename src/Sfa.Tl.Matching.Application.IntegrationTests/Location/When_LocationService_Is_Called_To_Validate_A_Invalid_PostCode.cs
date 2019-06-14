﻿using System.Net.Http;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_A_Invalid_PostCode
    {
        private readonly (bool, string) _result;

        public When_LocationService_Is_Called_To_Validate_A_Invalid_PostCode()
        {
            var locationService = new LocationApiClient(new HttpClient(), new MatchingConfiguration { PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes" });
            _result = locationService.IsValidPostCode("CV1234").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_False()
        {
            _result.Item1.Should().BeFalse();
            _result.Item2.Should().BeNullOrEmpty();
        }
    }
}