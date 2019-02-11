﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
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

            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderFileImportDto, ProviderDto>>(
                new NullLoggerFactory());

            MatchingDbContext = TestConfiguration.GetDbContext();

            var repository = new ProviderRepository(loggerRepository, MatchingDbContext);
            var dataValidator = new ProviderDataValidator(repository);
            var dataParser = new ProviderDataParser();

            var excelFileReader = new ExcelFileReader<ProviderFileImportDto, ProviderDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<ProviderMapper>());

            var mapper = new Mapper(config);

            ProviderService = new ProviderService(mapper, excelFileReader, repository);
        }

        //internal async Task ResetData()
        //{
        //    await MatchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.Provider");
        //    await MatchingDbContext.SaveChangesAsync();
        //}
        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}