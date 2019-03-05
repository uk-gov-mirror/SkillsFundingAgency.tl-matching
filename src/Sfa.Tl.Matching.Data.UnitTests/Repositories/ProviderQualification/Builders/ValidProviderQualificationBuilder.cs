﻿using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.ProviderQualification.Builders
{
    public class ValidProviderQualificationBuilder
    {
        private readonly Domain.Models.ProviderQualification _ProviderQualification;

        public ValidProviderQualificationBuilder()
        {
            _ProviderQualification = new Domain.Models.ProviderQualification
            {
                Id = 1,
                NumberOfPlacements = 1,
                ProviderVenueId = 1,
                QualificationId = 1,
                Source = "Test",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            };
        }

        public Domain.Models.ProviderQualification Build() =>
            _ProviderQualification;
    }
}
