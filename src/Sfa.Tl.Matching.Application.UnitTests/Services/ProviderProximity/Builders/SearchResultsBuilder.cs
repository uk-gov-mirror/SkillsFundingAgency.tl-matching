﻿using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderProximity.Builders
{
    public class SearchResultsBuilder
    {
        public IList<ProviderProximitySearchResultViewModelItem> Build() =>
            new List<ProviderProximitySearchResultViewModelItem>
            {
                new ProviderProximitySearchResultViewModelItem
                {
                    ProviderVenueId = 1,
                    ProviderName = "The WKCIC Group",
                    ProviderVenuePostcode = "NW1 3HB",
                    Distance = 2.5d,
                    Routes = new List<RouteAndQualificationsViewModel>
                    {
                        new RouteAndQualificationsViewModel
                        {
                            RouteId = 7,
                            RouteName = "health and science",
                            QualificationShortTitles = new List<string>
                            {
                                "applied science",
                                "health and physiotherapy"
                            }
                        }
                    }
                },
                new ProviderProximitySearchResultViewModelItem
                {
                    ProviderVenueId = 2,
                    ProviderName = "Lambeth College",
                    ProviderVenuePostcode = "SW4 9BL",
                    Distance = 2.5d,
                    Routes = new List<RouteAndQualificationsViewModel>
                    {
                        new RouteAndQualificationsViewModel
                        {
                            RouteId = 12,
                            RouteName = "care services",
                            QualificationShortTitles = new List<string>
                            {
                                "counselling skills"
                            }
                        }
                    }
                }
            };
    }
}
