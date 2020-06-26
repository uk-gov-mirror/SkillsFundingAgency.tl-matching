﻿namespace Sfa.Tl.Matching.Domain.Models
{
    public class ProviderVenueQualification
    {
        public long UkPrn { get; set; }
        public bool InMatchingService { get; set; }
        public string ProviderName { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool IsCDFProvider { get; set; }
        public bool IsEnabledForReferral { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactEmail { get; set; }
        public string PrimaryContactPhone { get; set; }
        public string SecondaryContact { get; set; }
        public string SecondaryContactEmail { get; set; }
        public string SecondaryContactPhone { get; set; }
        public string VenuePostcode { get; set; }
        public string Town { get; set; }
        public string VenueName { get; set; }
        public bool VenueIsEnabledForReferral { get; set; }
        public bool VenueIsRemoved { get; set; }
        public long LarId { get; set; }
        public string QualificationTitle { get; set; }
        public string QualificationShortTitle { get; set; }
        public bool QualificationIsDeleted { get; set; }
        public string Route { get; set; }
    }
}