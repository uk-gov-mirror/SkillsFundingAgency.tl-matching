﻿using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.EmailHistory.Builders
{
    public class ValidEmailHistoryListBuilder
    {
        public IList<Domain.Models.EmailHistory> Build() => new List<Domain.Models.EmailHistory>
        {
            new Domain.Models.EmailHistory
            {
                Id = 1,
                OpportunityId = 1,
                EmailTemplateId  = 2,
                SentTo = "recipient@test.com",
                CopiedTo = "copy@test.com",
                BlindCopiedTo = "blindcopy@test.com",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.EmailHistory
            {
                Id = 2,
                OpportunityId = 3,
                EmailTemplateId  = 4,
                SentTo = "recipient@test.com",
                CopiedTo = "copy@test.com",
                BlindCopiedTo = "blindcopy@test.com",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
