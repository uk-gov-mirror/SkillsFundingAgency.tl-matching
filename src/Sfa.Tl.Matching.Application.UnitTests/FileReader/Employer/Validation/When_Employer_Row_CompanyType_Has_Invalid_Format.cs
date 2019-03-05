﻿using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class When_Employer_Row_CompanyType_Has_Invalid_Format : IClassFixture<EmployerFileImportFixture>
    {
        private readonly EmployerFileImportFixture _fixture;

        public When_Employer_Row_CompanyType_Has_Invalid_Format(EmployerFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid()
        {
            _fixture.Dto.CompanyType = "ABC";
            
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            
            validationResult.IsValid.Should().BeFalse();
            
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.InvalidFormat.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(EmployerFileImportDto.CompanyType)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}