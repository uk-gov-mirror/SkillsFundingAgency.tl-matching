﻿using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class ProviderBuilder
    {
        private readonly MatchingDbContext _context;

        public ProviderBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Provider CreateProvider(int venueCount = 1)
        {
            var provider = new Provider
            {
                DisplayName = "test",
                Name = "test",
                OfstedRating = 1,
                PrimaryContact = "test",
                PrimaryContactEmail = "test@test.com",
                PrimaryContactPhone = "01234567890",
                SecondaryContact = "test",
                SecondaryContactEmail = "test@test.com",
                SecondaryContactPhone = "01234567890",
                Source = "test",
                UkPrn = 12345678,
                IsCdfProvider = true,
                IsEnabledForReferral = true,
                IsTLevelProvider = true,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                ProviderVenue = Enumerable.Range(1, venueCount).Select(venue =>
                    new ProviderVenue
                    {
                        IsRemoved = true,
                        IsEnabledForReferral = true,
                        County = "test" + venue,
                        Name = "test" + venue,
                        Postcode = "test" + venue,
                        Source = "test" + venue,
                        Town = "test" + venue,
                        CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                        ProviderQualification = new List<ProviderQualification>
                        {
                            new ProviderQualification
                            {
                                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                NumberOfPlacements = 1,
                                Source = "test",
                                Qualification = new Qualification
                                {
                                    CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                    LarsId = "12345678",
                                    ShortTitle = "test",
                                    Title = "test",
                                    QualificationRouteMapping = new List<QualificationRouteMapping>
                                    {
                                        new QualificationRouteMapping
                                        {
                                            RouteId = 1,
                                            CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                                            Source = "test",
                                        }
                                    }
                                }
                            }
                        }
                    }).ToList()
            };

            _context.Add(provider);

            _context.SaveChanges();

            return provider;
        }

        public void ClearData()
        {
            var qrm = _context.QualificationRouteMapping.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!qrm.IsNullOrEmpty()) _context.QualificationRouteMapping.RemoveRange(qrm);

            var qual = _context.Qualification.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!qual.IsNullOrEmpty()) _context.Qualification.RemoveRange(qual);

            var pq = _context.ProviderQualification.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!pq.IsNullOrEmpty()) _context.ProviderQualification.RemoveRange(pq);

            var pv = _context.ProviderVenue.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!pv.IsNullOrEmpty()) _context.ProviderVenue.RemoveRange(pv);

            var p = _context.Provider.Where(q => q.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!p.IsNullOrEmpty()) _context.Provider.RemoveRange(p);

            _context.SaveChanges();
        }
    }
}