using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Employer
{
    public class When_Employer_FindEmployer_Is_Loaded
    {
        private readonly IActionResult _result;
        private const int OpportunityId = 12;
        private const int EmployerId = 1;

        private const string EmployerName = "EmployerName";

        public When_Employer_FindEmployer_Is_Loaded()
        {
            var employerService = Substitute.For<IEmployerService>();
            var opportunityService = Substitute.For<IOpportunityService>();
            opportunityService.GetOpportunity(OpportunityId).Returns(new OpportunityDto
            {
                Id = OpportunityId,
                EmployerName = EmployerName,
                EmployerId = EmployerId
            });
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerDtoMapper).Assembly));
            var mapper = new Mapper(config);

            var employerController = new EmployerController(employerService, opportunityService, mapper);

            _result = employerController.FindEmployer(OpportunityId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_OpportunityId_Is_Set()
        {
            var viewModel = _result.GetViewModel<FindEmployerViewModel>();
            viewModel.OpportunityId.Should().Be(OpportunityId);
        }

        [Fact]
        public void Then_EmployerId_Is_Set()
        {
            var viewModel = _result.GetViewModel<FindEmployerViewModel>();
            viewModel.SelectedEmployerId.Should().Be(EmployerId);
        }

        [Fact]
        public void Then_EmployerName_Is_Populated()
        {
            var viewModel = _result.GetViewModel<FindEmployerViewModel>();
            viewModel.EmployerName.Should().Be(EmployerName);
        }
    }
}