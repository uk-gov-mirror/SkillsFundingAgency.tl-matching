using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Path.Builders;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Path
{
    public class When_PathRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id
    {
        private readonly Domain.Models.Path _result;

        public When_PathRepository_GetSingleOrDefault_Is_Called_With_Non_Existent_Id()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.Path>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.Add(new ValidPathBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.Path>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 2)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_No_Path_Is_Returned() =>
            _result.Should().BeNull();
    }
}