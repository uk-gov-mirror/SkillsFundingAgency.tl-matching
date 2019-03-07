using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailTemplate
{
    public class When_EmailTemplateRepository_GetSingleOrDefault_Is_Called
    {
        private readonly Domain.Models.EmailTemplate _result;

        public When_EmailTemplateRepository_GetSingleOrDefault_Is_Called()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Domain.Models.EmailTemplate>>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidEmailTemplateListBuilder().Build());
                dbContext.SaveChanges();

                var repository = new GenericRepository<Domain.Models.EmailTemplate>(logger, dbContext);
                _result = repository.GetSingleOrDefault(x => x.Id == 1)
                    .GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_EmailTemplate_Id_Is_Returned() =>
            _result.Id.Should().Be(1);
        
        [Fact]
        public void Then_EmailTemplate_TemplateId_Is_Returned() =>
            _result.TemplateId.Should().BeEquivalentTo("TestTemplateId");

        [Fact]
        public void Then_EmailTemplate_TemplateName_Is_Returned() =>
            _result.TemplateName.Should().BeEquivalentTo("TestTemplate");
        
        [Fact]
        public void Then_EmailTemplate_Recipients_Is_Returned()
            => _result.Recipients.Should().BeEquivalentTo("test@test.com");

        [Fact]
        public void Then_EmailTemplate_CreatedBy_Is_Returned() =>
            _result.CreatedBy.Should().BeEquivalentTo(EntityCreationConstants.CreatedByUser);

        [Fact]
        public void Then_EmailTemplate_CreatedOn_Is_Returned() =>
            _result.CreatedOn.Should().Be(EntityCreationConstants.CreatedOn);

        [Fact]
        public void Then_EmailTemplate_ModifiedBy_Is_Returned() =>
            _result.ModifiedBy.Should().BeEquivalentTo(EntityCreationConstants.ModifiedByUser);
        
        [Fact]
        public void Then_EmailTemplate_ModifiedOn_Is_Returned() =>
            _result.ModifiedOn.Should().Be(EntityCreationConstants.ModifiedOn);
    }
}