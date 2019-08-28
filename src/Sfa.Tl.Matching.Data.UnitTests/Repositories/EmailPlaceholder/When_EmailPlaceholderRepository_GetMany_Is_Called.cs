using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailPlaceholder
{
    public class When_EmailPlaceholderRepository_GetMany_Is_Called
    {
        private readonly IEnumerable<Domain.Models.EmailPlaceholder> _result;

        public When_EmailPlaceholderRepository_GetMany_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailPlaceholder>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidEmailPlaceholderListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailPlaceholder>(logger, dbContext);
                _result = repository.GetMany().ToList();
            }
        }

        [Fact]
        public void Then_The_Expected_Number_Of_EmailPlaceholders_Is_Returned() =>
            _result.Count().Should().Be(2);

        [Fact]
        public void Then_First_EmailHistory_Fields_Have_Expected_Values()
        {
            _result.First().Id.Should().Be(1);
            _result.First().EmailHistoryId.Should().Be(1);
            _result.First().Key.Should().BeEquivalentTo("name_Placeholder");
            _result.First().Value.Should().BeEquivalentTo("Name");
            _result.First().CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);
            _result.First().CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);
            _result.First().ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
            _result.First().ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
        }
    }
}