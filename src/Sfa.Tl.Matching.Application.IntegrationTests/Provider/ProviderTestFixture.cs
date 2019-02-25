﻿using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.SearchProviders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class ProviderTestFixture : IDisposable
    {
        internal readonly IProviderService ProviderService;
        internal MatchingDbContext MatchingDbContext;

        public ProviderTestFixture()
        {
            var loggerRepository = new Logger<ProviderRepository>(
                new NullLoggerFactory());

            var loggerSearchProvider = new Logger<SqlSearchProvider>(
                new NullLoggerFactory());

            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderFileImportDto, ProviderDto>>(
                new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var repository = new ProviderRepository(loggerRepository, MatchingDbContext);
            var dataValidator = new ProviderDataValidator(repository);
            var dataParser = new ProviderDataParser();

            var excelFileReader = new ExcelFileReader<ProviderFileImportDto, ProviderDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<ProviderMapper>());
            var mapper = new Mapper(config);
            var searchResultconfig = new MapperConfiguration(c => c.AddProfile<ProviderVenueSearchResultMapper>());
            var searchResultMapper = new Mapper(searchResultconfig);
            var searchProvider = new SqlSearchProvider(loggerSearchProvider, MatchingDbContext);

            ProviderService = new ProviderService(mapper, excelFileReader, repository, searchResultMapper, searchProvider);
        }

        internal void ResetData(string name)
        {
            var provider = MatchingDbContext.Provider.FirstOrDefault(p => p.Name == name);
            if (provider != null)
            {
                MatchingDbContext.Provider.Remove(provider);
                var count = MatchingDbContext.SaveChanges();
                count.Should().Be(1);
            }
        }

        internal int GetCountBy(string name)
        {
            var providerCount = MatchingDbContext.Provider.Count(p => p.Name == name);

            return providerCount;
        }

        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}