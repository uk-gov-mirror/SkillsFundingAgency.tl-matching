﻿using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.ProviderVenue;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderVenue
{
    public class ProviderVenueTestFixture : IDisposable
    {
        internal readonly IProviderVenueService ProviderVenueService;
        internal MatchingDbContext MatchingDbContext;

        public ProviderVenueTestFixture()
        {
            var loggerRepository = new Logger<ProviderRepository>(
                new NullLoggerFactory());
            var providerVenueloggerRepository = new Logger<ProviderVenueRepository>(
                new NullLoggerFactory());

            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>>(
                new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var repository = new ProviderRepository(loggerRepository, MatchingDbContext);
            var providerVenuerepository = new ProviderVenueRepository(providerVenueloggerRepository, MatchingDbContext);
            var dataValidator = new ProviderVenueDataValidator(repository, providerVenuerepository);
            var dataParser = new ProviderVenueDataParser();

            var excelFileReader = new ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<ProviderVenueMapper>());

            var mapper = new Mapper(config);

            ProviderVenueService = new ProviderVenueService(mapper, excelFileReader, providerVenuerepository);
        }

        internal void ResetData(int ukprn)
        {
            ResetProviderVenue(ukprn);
            ResetProvider(ukprn);
        }

        internal Domain.Models.Provider CreateProvider(int ukprn)
        {
            var provider = new Domain.Models.Provider
            {
                UkPrn = ukprn,
                Name = nameof(ProviderVenueTestFixture),
                OfstedRating = 3,
                Status = true,
                PrimaryContact = "PrimaryContact",
                PrimaryContactEmail = "primary@contact.com",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.com",
                Source = nameof(ProviderVenueTestFixture),
                CreatedOn = DateTime.Now
            };

            MatchingDbContext.Add(provider);
            MatchingDbContext.SaveChanges();

            return provider;
        }

        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }

        private void ResetProviderVenue(int ukprn)
        {
            var providerVenue = MatchingDbContext.ProviderVenue.FirstOrDefault(pv => pv.Provider.UkPrn == ukprn);
            if (providerVenue != null)
            {
                MatchingDbContext.ProviderVenue.Remove(providerVenue);
                var count = MatchingDbContext.SaveChanges();
                count.Should().Be(1);
            }
        }

        private void ResetProvider(int ukprn)
        {
            var provider = MatchingDbContext.Provider.FirstOrDefault(p => p.UkPrn == ukprn);
            if (provider != null)
            {
                MatchingDbContext.Provider.Remove(provider);
                var count = MatchingDbContext.SaveChanges();
                count.Should().Be(1);
            }
        }
    }
}