﻿using FluentValidation.Results;
using Humanizer;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_No_Title
    {
        private ValidationResult _validationResult;

        
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var dto = routePathMapping.ToDto();
            dto.Title = "";

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();

            var validator = new QualificationRoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Fact]
        public void Then_Error_Count_Is_One() =>
            Assert.Equal(1, _validationResult.Errors.Count);

        [Fact]
        public void Then_Error_Code_Is_MissingMandatoryData() =>
            Assert.Equal(ValidationErrorCode.MissingMandatoryData.ToString(), _validationResult.Errors[0].ErrorCode);

        [Fact]
        public void Then_Error_Message_Is_MissingMandatoryData() =>
            Assert.Equal($"'Title' - {ValidationErrorCode.MissingMandatoryData.Humanize()}", _validationResult.Errors[0].ErrorMessage);
    }
}