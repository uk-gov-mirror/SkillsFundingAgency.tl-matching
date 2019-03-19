﻿using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class PlacementInformationSaveViewModel
    {
        public int OpportunityId { get; set; }

        [Required(ErrorMessage = "You must tell us what specific job the placement student would do")]
        [MinLength(2, ErrorMessage = "You must enter a job role using 2 or more characters")]
        [MaxLength(99, ErrorMessage = "You must enter a job role using 99 characters or less")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "You must tell us whether the employer knows how many placements they want at this location")]
        public bool? PlacementsKnown { get; set; }

        public int? Placements { get; set; }

        public int RouteId { get; set; }
        public string Postcode { get; set; }
        public short Distance { get; set; }
        public string ModifiedBy { get; set; } // TODO AU REMOVE THIS
    }
}