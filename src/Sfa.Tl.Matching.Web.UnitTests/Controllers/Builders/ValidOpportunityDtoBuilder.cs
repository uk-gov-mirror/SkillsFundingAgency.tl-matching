﻿using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityDtoBuilder
    {
        private readonly OpportunityDto _dto;

        public ValidOpportunityDtoBuilder()
        {
            _dto = new OpportunityDto
            {
                Id = 1,
                Distance = 3,
                JobTitle = "JobTitle",
                PlacementsKnown = true,
                Placements = 2,
                Postcode = "AA1 1AA",
                EmployerName = "EmployerName",
                EmployerContact = "Contact",
                EmployerContactEmail = "ContactEmail",
                EmployerContactPhone = "ContactPhone",
                RouteName = "RouteName",
                ModifiedBy = "ModifiedBy"
            };
        }

        public OpportunityDto Build() =>
            _dto;
    }
}