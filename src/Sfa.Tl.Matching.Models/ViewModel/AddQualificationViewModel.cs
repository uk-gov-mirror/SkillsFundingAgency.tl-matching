﻿using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class AddQualificationViewModel
    {
        public int ProviderVenueId { get; set; }
        public string Postcode { get; set; }
        [Required(ErrorMessage = "You must enter a LAR ID")]
        public string LarsId { get; set; }
    }
}