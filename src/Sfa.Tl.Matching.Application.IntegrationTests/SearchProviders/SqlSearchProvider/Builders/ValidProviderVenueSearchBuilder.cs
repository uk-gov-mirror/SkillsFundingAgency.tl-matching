﻿using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.IntegrationTests.SearchProviders.SqlSearchProvider.Builders
{
    public class ValidProviderVenueSearchBuilder
    {
        private const decimal Latitude = 52.400997m;
        private const decimal Longitude = -1.508122m;

        public ProviderVenue BuildOneVenue()
        {
            var location = CreatePointLocation((double)Latitude, (double)Longitude);

            return new ProviderVenue
            {
                Provider = BuildProvider(true),
                Postcode = "CV1 2WT",
                Latitude = Latitude,
                Longitude = Longitude,
                Location = location,
                IsEnabledForReferral = true,
                IsRemoved = false,
                Source = "Test",
                ProviderQualification = BuildProviderQualifications()
            };
        }

        public ProviderVenue BuildOneVenueWithDisabledProvider()
        {
            var location = CreatePointLocation((double)Latitude, (double)Longitude);

            return new ProviderVenue
            {
                Provider = BuildProvider(false),
                Postcode = "CV1 2WT",
                Latitude = Latitude,
                Longitude = Longitude,
                Location = location,
                IsEnabledForReferral = true,
                IsRemoved = false,
                Source = "Test",
                ProviderQualification = BuildProviderQualifications()
            };
        }

        public IList<ProviderVenue> BuildWithOneVenueEnabled()
        {
            var location = CreatePointLocation((double)Latitude, (double)Longitude);

            return new List<ProviderVenue>
            {
                new ProviderVenue
                {
                    Provider = BuildProvider(true),
                    Postcode = "CV1 2WT",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications()
                },
                new ProviderVenue
                {
                    Provider = BuildProvider(true),
                    Postcode = "CV1 2WT",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = false,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications()
                }
            };
        }

        public IList<ProviderVenue> BuildWithTwoVenuesEnabled()
        {
            var location = CreatePointLocation((double)Latitude, (double)Longitude);

            return new List<ProviderVenue>
            {
                new ProviderVenue
                {
                    Provider = BuildProvider(true),
                    Postcode = "CV1 2WT",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications()
                },
                new ProviderVenue
                {
                    Provider = BuildProvider(true),
                    Postcode = "CV1 2WT",
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Location = location,
                    IsEnabledForReferral = true,
                    IsRemoved = false,
                    Source = "Test",
                    ProviderQualification = BuildProviderQualifications()
                }
            };
        }

        private static IPoint CreatePointLocation(double latitude, double longitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
            return geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
        }

        private static Provider BuildProvider(bool isCdfProvider)
        {
            return new Provider
            {
                UkPrn = 10203040,
                Name = "SQL Search Provider",
                PrimaryContact = "Test",
                PrimaryContactEmail = "Test@test.com",
                PrimaryContactPhone = "0123456789",
                SecondaryContact = "Test 2",
                SecondaryContactEmail = "Test2@test.com",
                SecondaryContactPhone = "0123456789",
                IsCdfProvider = isCdfProvider,
                IsEnabledForReferral = true,
                Source = "Test"
            };
        }

        private static ICollection<ProviderQualification> BuildProviderQualifications()
        {
            return new List<ProviderQualification>
            {
                new ProviderQualification
                {
                    Qualification = new Qualification
                    {
                        LarsId = "12345678",
                        Title = "Qualification Title",
                        ShortTitle = "Short Title",
                        QualificationRouteMapping = new List<QualificationRouteMapping>
                        {
                            new QualificationRouteMapping
                            {
                                RouteId = 7,
                                Source = "Test"
                            }
                        }
                    },
                    Source = "Test"
                }
            };
        }
    }
}