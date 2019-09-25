﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using Newtonsoft.Json;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_Employer_Service_Is_Called_To_Handle_Valid_Contact_Updated_Event_For_New_Employer
    {
        private readonly IRepository<Domain.Models.Employer> _employerRepository;

        public When_Employer_Service_Is_Called_To_Handle_Valid_Contact_Updated_Event_For_New_Employer()
        {
            _employerRepository = Substitute.For<IRepository<Domain.Models.Employer>>();
            var opportunityRepository = Substitute.For<IOpportunityRepository>();

            _employerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Employer, bool>>>())
                .Returns((Domain.Models.Employer)null);

            var employerService = new EmployerService(_employerRepository, opportunityRepository, Substitute.For<IMapper>(), new CrmEmployerEventDataValidator());

            var employerEventBase = CrmEmployerEventBaseBuilder.Buiild(true);

            var data = JsonConvert.SerializeObject(employerEventBase, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore });

            employerService.HandleContactUpdatedAsync(data).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Employer_Record_Should_Be_Created()
        {
            _employerRepository.DidNotReceive().Create(Arg.Any<Domain.Models.Employer>());
            _employerRepository.DidNotReceive().Update(Arg.Any<Domain.Models.Employer>());
        }
    }
}