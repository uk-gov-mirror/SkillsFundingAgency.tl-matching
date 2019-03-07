﻿using System.Collections.Generic;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IMapper _searchResultMapper;
        private readonly ISearchProvider _searchProvider;

        public ProviderService(
            IMapper searchResultMapper,
            ISearchProvider searchProvider)
        {
            _searchResultMapper = searchResultMapper;
            _searchProvider = searchProvider;
        }

        public async Task<IEnumerable<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto)
        {

            var searchResults = _searchProvider.SearchProvidersByPostcodeProximity(dto);

            var results = searchResults.Any()
                ? _searchResultMapper.Map<IEnumerable<ProviderVenueSearchResultDto>>(searchResults)
                : new List<ProviderVenueSearchResultDto>();

            return results;
        }
    }
}