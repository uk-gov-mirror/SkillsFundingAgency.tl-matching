﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_Import_ProviderVenues
    {
        private readonly ProviderVenueFileImportDto _fileImportDto;
        private readonly IEnumerable<ProviderVenueDto> _fileReaderResults;
        private readonly IFileReader<ProviderVenueFileImportDto, ProviderVenueDto> _fileReader;
        private readonly IRepository<Domain.Models.ProviderVenue> _repository;
        private readonly int _result;

        public When_ProviderVenueService_Is_Called_To_Import_ProviderVenues()
        {
            var config = new MapperConfiguration(c => c.AddProfile<ProviderVenueMapper>());
            var mapper = new Mapper(config);
            _fileReader =
                Substitute.For<IFileReader<ProviderVenueFileImportDto, ProviderVenueDto>>();
            _repository = Substitute.For<IRepository<Domain.Models.ProviderVenue>>();

            _repository
                .CreateMany(Arg.Any<IEnumerable<Domain.Models.ProviderVenue>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<Domain.Models.ProviderVenue>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new ProviderVenueFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = new ValidProviderVenueDtoListBuilder(2).Build();

            _fileReader.ValidateAndParseFile(_fileImportDto)
                .Returns(_fileReaderResults);

            var service = new ProviderVenueService(mapper, _fileReader, _repository);

            _result = service.ImportProviderVenue(_fileImportDto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ValidateAndParseFile_Is_Called_Exactly_Once()
        {
            _fileReader
                .Received(1)
                .ValidateAndParseFile(_fileImportDto);
        }

        [Fact]
        public void Then_CreateMany_Is_Called_Exactly_Once()
        {
            _repository
                .Received(1)
                .CreateMany(Arg.Any<IEnumerable<Domain.Models.ProviderVenue>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            Assert.Equal(_fileReaderResults.Count(), _result);
        }
    }
}
