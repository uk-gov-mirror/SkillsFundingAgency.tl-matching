﻿using AutoMapper;

using Sfa.Tl.Matching.Application.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers
{
    public class When_CreateEmployerDto_Is_Mapped_To_Employer
    {
        private MapperConfiguration _config;

        
        public void Setup()
        {
            _config = new MapperConfiguration(c => c.AddProfile<EmployerMapper>());
        }

        [Fact]
        public void Then_All_Properties_Are_Implemented() =>
            _config.AssertConfigurationIsValid();
    }
}