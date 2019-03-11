﻿using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory.Builders
{
    public class ValidEmailHistoryBuilder
    {
        public Domain.Models.EmailHistory Build() => new Domain.Models.EmailHistory
        {
            Id = 1,
            ReferralId = 1,
            EmailTemplateId = 2,
            SentTo = "recipient@test.com",
            CopiedTo = "copy@test.com",
            BlindCopiedTo = "blindcopy@test.com",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
