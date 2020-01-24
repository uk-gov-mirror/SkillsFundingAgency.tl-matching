﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider
{
    public class When_SqlSearchProvider_Search_Providers_For_Report_Is_Called_With_Valid_Parameters_With_Two_Venues_Enabled : IDisposable
    {
        private readonly ProviderProximityReportDto _results;
        private readonly MatchingDbContext _dbContext;
        private readonly IList<ProviderVenue> _providerVenues;

        public When_SqlSearchProvider_Search_Providers_For_Report_Is_Called_With_Valid_Parameters_With_Two_Venues_Enabled()
        {
            var logger = Substitute.For<ILogger<Data.SearchProviders.SqlSearchProvider>>();

            _dbContext = new TestConfiguration().GetDbContext();

            _providerVenues = new ValidProviderVenueSearchBuilder().BuildWithTwoVenuesEnabled();
            _dbContext.AddRange(_providerVenues);
            _dbContext.SaveChanges();

            var provider = new Data.SearchProviders.SqlSearchProvider(logger, _dbContext);

            _results = provider.SearchProvidersByPostcodeProximityForReportAsync(
                new ProviderProximitySearchParametersDto
                {
                    Postcode = "CV1 2WT", 
                    SearchRadius = 5, 
                    SelectedRoutes = null, 
                    Latitude = "52.400997", 
                    Longitude = "-1.508122"
                }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Exactly_One_Provider_Is_Found_Within_Search_Radius()
        {
            _results.Should().NotBeNull();
            _results.Providers.Select(p => p.ProviderName).Distinct().Count().Should().Be(1);
            _results.Providers.Count.Should().Be(2);

            var firstItem = _results.Providers.First();
            firstItem.ProviderName.Should().Be("SQL Search Provider (CV1 2WT)");
            firstItem.ProviderDisplayName.Should().Be("SQL Search Provider");

            firstItem.ProviderVenuePostcode.Should().Be("CV1 2WT");
            firstItem.ProviderVenueName.Should().Be("CV1 2WT");
            firstItem.ProviderVenueTown.Should().Be("Coventry");
            firstItem.Distance.Should().Be(0);
            firstItem.PrimaryContact.Should().Be("Test");
            firstItem.PrimaryContactEmail.Should().Be("Test@test.com");
            firstItem.PrimaryContactPhone.Should().Be("0123456789");
            firstItem.SecondaryContact.Should().Be("Test 2");
            firstItem.SecondaryContactEmail.Should().Be("Test2@test.com");
            firstItem.SecondaryContactPhone.Should().Be("0987654321");

            firstItem.Routes.Count().Should().Be(1);
            
            var route = firstItem.Routes.First();
            route.RouteId.Should().Be(7);
            route.RouteName.Should().Be("Education and childcare");

            route.QualificationShortTitles.Count().Should().Be(1);
            var qualification = route.QualificationShortTitles.First();

            qualification.Should().Be("Short Title");

            var secondItem = _results.Providers.Skip(1).First();
            secondItem.ProviderName.Should().Be("SQL Search Provider (CV1 2WT)");
            secondItem.ProviderDisplayName.Should().Be("SQL Search Provider");
        }

        public void Dispose()
        {
            var providerQualifications = _providerVenues.SelectMany(p => p.ProviderQualification).ToList();

            var qualificationMappings = providerQualifications.SelectMany(q => q.Qualification.QualificationRouteMapping).ToList();
            var qualifications = providerQualifications.Select(p => p.Qualification).ToList();

            _dbContext.RemoveRange(providerQualifications);
            _dbContext.RemoveRange(_providerVenues);
            _dbContext.RemoveRange(_providerVenues.Select(p => p.Provider));
            _dbContext.RemoveRange(qualificationMappings);
            _dbContext.RemoveRange(qualifications);

            _dbContext.SaveChanges();
            _dbContext.Dispose();
        }
    }
}
