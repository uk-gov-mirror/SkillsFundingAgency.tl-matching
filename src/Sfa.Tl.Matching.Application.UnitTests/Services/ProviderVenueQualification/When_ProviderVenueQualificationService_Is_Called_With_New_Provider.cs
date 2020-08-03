﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualification.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenueQualification
{
    public class When_ProviderVenueQualificationService_Is_Called_With_New_Provider
    {
        private readonly IProviderService _providerService;
        private readonly IProviderVenueService _providerVenueService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IQualificationService _qualificationService;
        private readonly IRoutePathService _routePathService;
        private readonly IQualificationRouteMappingService _qualificationRouteMappingService;

        private readonly IEnumerable<ProviderVenueQualificationUpdateResultsDto> _results;

        public When_ProviderVenueQualificationService_Is_Called_With_New_Provider()
        {
            _providerService = Substitute.For<IProviderService>();
            _providerVenueService = Substitute.For<IProviderVenueService>();
            _providerQualificationService = Substitute.For<IProviderQualificationService>();
            _qualificationService = Substitute.For<IQualificationService>();
            _routePathService = Substitute.For<IRoutePathService>();
            _qualificationRouteMappingService = Substitute.For<IQualificationRouteMappingService>();

            _providerService.SearchAsync(10000001)
                .Returns((ProviderSearchResultDto) null);

            _providerService.CreateProviderAsync(Arg.Any<CreateProviderDetailViewModel>()).Returns(1);

            var providerVenueQualificationService = new ProviderVenueQualificationService
                (
                   _providerService,
                   _providerVenueService,
                   _providerQualificationService,
                   _qualificationService,
                    _routePathService,
                   _qualificationRouteMappingService
                );

            var dto = new ValidProviderVenueQualificationFileImportDtoListBuilder().Build();

            _results = providerVenueQualificationService.Update(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Results_Has_One_Item()
        {
            _results.Should().NotBeNull();
            var resultsList = _results.ToList();
            resultsList.Count.Should().Be(1);
        }

        [Fact]
        public void Then_ProviderService_SearchAsync_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SearchAsync(Arg.Any<long>());
        }

        [Fact]
        public void Then_ProviderService_GetProviderDetailByIdAsync_Is_Not_Called()
        {
            _providerService
                .DidNotReceive()
                .GetProviderDetailByIdAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_ProviderService_CreateProviderAsync_Is_Called_Exactly_Once()
        {
            _providerService.Received(1)
                .CreateProviderAsync(Arg.Any<CreateProviderDetailViewModel>());
        }

        [Fact]
        public void Then_ProviderService_CreateProviderAsync_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _providerService.Received(1)
                .CreateProviderAsync(Arg.Is<CreateProviderDetailViewModel>(
                    p => p.UkPrn == 10000001 &&
                         p.Name == "Test Provider Name" &&
                         p.DisplayName == "Test Provider Display Name" &&
                         p.IsCdfProvider &&
                         p.IsEnabledForReferral.HasValue && p.IsEnabledForReferral.Value &&
                         p.PrimaryContact == "test primary contact" &&
                         p.PrimaryContactEmail == "testprimary@test.com" &&
                         p.PrimaryContactPhone == "01234567890" &&
                         p.SecondaryContact == "test secondary contact" &&
                         p.SecondaryContactEmail == "testsecondary@test.com" &&
                         p.SecondaryContactPhone == "01234567891"));
        }
    }
}
