﻿using OpenQA.Selenium;

namespace Sfa.Tl.Matching.Web.IntegrationTests.PageObjects.Opportunity
{
    public class OpportunityBasketPage : PageBase
    {
        private readonly By _addOpportunityLink = By.CssSelector("govuk-button tl-button--grey");
        private readonly By _continueButton = By.Id("tl-continue");

        public OpportunityBasketPage(IWebDriver driver) : base(driver)
        {
        }
    }
}