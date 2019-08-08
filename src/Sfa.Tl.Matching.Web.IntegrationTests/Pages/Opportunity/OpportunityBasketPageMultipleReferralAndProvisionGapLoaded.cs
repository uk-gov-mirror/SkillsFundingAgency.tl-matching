﻿using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using Sfa.Tl.Matching.Web.IntegrationTests.Helpers;
using Sfa.Tl.Matching.Web.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Pages.Opportunity
{
    public class OpportunityBasketPageMultipleReferralAndProvisionGapLoaded : IClassFixture<CustomWebApplicationFactory<TestStartup>>
    {
        private const string Title = "All opportunities";
        private const int OpportunityId = 1050;
        private const int OpportunityItem1Id = 1051;
        private const int OpportunityItem2Id = 1052;
        private const int OpportunityItem3Id = 1053;

        private readonly CustomWebApplicationFactory<TestStartup> _factory;

        public OpportunityBasketPageMultipleReferralAndProvisionGapLoaded(CustomWebApplicationFactory<TestStartup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task ReturnsCorrectResponse()
        {
            // ReSharper disable all PossibleNullReferenceException

            var client = _factory.CreateClient();
            var response = await client.GetAsync($"employer-opportunities/{OpportunityId}-{OpportunityItem1Id}");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var documentHtml = await HtmlHelpers.GetDocumentAsync(response);

            documentHtml.Title.Should().Be($"{Title} - {Constants.ServiceName} - GOV.UK");

            var header1 = documentHtml.QuerySelector(".govuk-heading-l");
            header1.TextContent.Should().Be(Title);

            var employerName = documentHtml.QuerySelector(".govuk-caption-l");
            employerName.TextContent.Should().Be("Company Name");

            var tabList = documentHtml.QuerySelector(".govuk-tabs__list");
            var providerTab = tabList.Children[0] as IHtmlListItemElement;
            providerTab.Text().Should().Be("\n            \n                With providers\n            \n        ");

            var noProviderTab = tabList.Children[1] as IHtmlListItemElement;
            noProviderTab.Text().Should().Be("\n            \n                With no providers\n            \n        ");

            var providerBasketTable = documentHtml.QuerySelector("#providers .govuk-table") as IHtmlTableElement;
            var providerRow = providerBasketTable.Rows[1];
            providerRow.Cells[1].TextContent.Should().Be("London SW1A 2AA");
            providerRow.Cells[2].TextContent.Should().Be("Job Role");
            providerRow.Cells[3].TextContent.Should().Be("1");
            providerRow.Cells[4].TextContent.Should().Be("1");

            var providerEditCell = providerRow.Cells[5].Children[0] as IHtmlAnchorElement;
            providerEditCell.Text.Should().Be("Edit");
            providerEditCell.PathName.Should().Be($"/check-answers/{OpportunityItem1Id}");

            var providerDeleteCell = providerRow.Cells[6].Children[0] as IHtmlAnchorElement;
            providerDeleteCell.Text.Should().Be("Delete");
            providerDeleteCell.PathName.Should().Be($"/remove-opportunity/{OpportunityItem1Id}");

            var addAnotherLink = documentHtml.QuerySelector("#tl-add-another-opportunity") as IHtmlAnchorElement;
            addAnotherLink.Text.Should().Be("Add another opportunity");
            addAnotherLink.PathName.Should().Be($"/find-providers/{OpportunityId}");

            var continueButton = documentHtml.QuerySelector("#tl-continue") as IHtmlButtonElement;
            continueButton.Name.Should().Be("SubmitAction");
            continueButton.Value.Should().Be("SaveSelectedOpportunities");
            continueButton.TextContent.Should().Be("Continue with selected opportunities");

            var noProviderBasketTable = documentHtml.QuerySelector("#no-providers .govuk-table") as IHtmlTableElement;
            var noProviderRow = noProviderBasketTable.Rows[1];
            noProviderRow.Cells[0].TextContent.Should().Be("London SW1A 2AA");
            noProviderRow.Cells[1].TextContent.Should().Be("Job Role");
            noProviderRow.Cells[2].TextContent.Should().Be("1");
            noProviderRow.Cells[3].TextContent.Should().Be("Employer had a bad experience with them");
        }
    }
}