﻿using System;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database.StandingData
{
    internal class EmployerLoad
    {
        public static Employer[] Create()
        {
            var employers = new[]
            {
                new Employer
                {
                    Id = 1,
                    CrmId = new Guid("65351B3C-FAF8-4752-8806-8D6E240C334E"),
                    CompanyName = "Company Name",
                    AlsoKnownAs = "Also Known As",
                    CompanyNameSearch = "CompanyNameAlsoKnownAs",
                    Aupa = "Active",
                    CompanyType = "Employer",
                    PrimaryContact = "Primary Contact",
                    Phone = "07878 787558",
                    Email = "email@address.com",
                    Postcode = "CV1 2WT",
                    Owner = "Owner",
                    CreatedOn = new DateTime(2019, 1, 1)
                }
            };

            return employers;
        }
    }
}