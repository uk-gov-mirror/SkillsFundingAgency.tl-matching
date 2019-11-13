using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity
{
    public class When_OpportunityRepository_GetReferralsForEmployerFeedback_Is_Called_With_One_Abandoned_Opportunity : IDisposable
    {
        private readonly MatchingDbContext _dbContext;
        private readonly IList<EmployerFeedbackDto> _result;
        private readonly int _opportunityId;
        private readonly int _opportunityItemId;

        public When_OpportunityRepository_GetReferralsForEmployerFeedback_Is_Called_With_One_Abandoned_Opportunity()
        {
            var logger = Substitute.For<ILogger<OpportunityRepository>>();

            _dbContext = InMemoryDbContext.Create();

            var opportunity = new ValidOpportunityBuilder()
                .AddEmployer()
                .AddReferrals(true)
                .AddAbandonedOpportunityItem()
                .Build();

            _dbContext.Add(opportunity);
            _dbContext.SaveChanges();

            //Save the ids to avoid interaction with other tests breaking the assertions
            _opportunityId = opportunity.Id;
            _opportunityItemId = opportunity.OpportunityItem.First().Id;

            var repository = new OpportunityRepository(logger, _dbContext);
            var referralDate = EntityCreationConstants.ModifiedOn.AddDays(1);
            _result = repository.GetReferralsForEmployerFeedbackAsync(referralDate)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmployerFeedbackDto_Fields_Are_As_Expected()
        {
            _result.Should().NotBeNullOrEmpty();
            _result.Count.Should().Be(1);
            _result.First().OpportunityId.Should().Be(_opportunityId);
            _result.First().OpportunityItemId.Should().Be(_opportunityItemId);
            _result.First().EmployerContact.Should().BeEquivalentTo("Employer Contact");
            _result.First().EmployerContactEmail.Should().BeEquivalentTo("employer.contact@employer.co.uk");
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}