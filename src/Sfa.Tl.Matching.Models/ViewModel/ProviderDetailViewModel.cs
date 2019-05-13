﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class ProviderDetailViewModel
    {
        public ProviderDetailViewModel()
        {
            ProviderVenue = new List<ProviderVenueViewModel>();
        }

        public int Id { get; set; }
        public long UkPrn { get; set; }
        public string Name { get; set; }

        [MaxLength(400, ErrorMessage = "You must enter a provider name that is 400 characters or fewer")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "You must tell us who the primary contact is for industry placements")]
        [MinLength(2, ErrorMessage = "You must enter a primary contact name using 2 or more characters")]
        [MaxLength(99, ErrorMessage = "You must enter a contact name that is 100 characters or fewer")]
        [RegularExpression(@"^(?!^\d+$)^.+$", ErrorMessage = "You must enter a contact name using letters")]
        public string PrimaryContact { get; set; }

        [Required(ErrorMessage = "You must enter an email for the primary contact")]
        [RegularExpression(@"^[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+@[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+\.([a-zA-Z0-9\u0080-\uFFA7]{2,10})$", ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        public string PrimaryContactEmail { get; set; }
        
        [PhoneNumber(FieldName = "primary contact", IsRequired = true)]
        public string PrimaryContactPhone { get; set; }

        [RegularExpression(@"^(?!^\d+$)^.+$", ErrorMessage = "You must enter a contact name using letters")]
        [MinLength(2, ErrorMessage = "You must enter a contact name using 2 or more characters")]
        [MaxLength(99, ErrorMessage = "You must enter a contact name that is 100 characters or fewer")]
        public string SecondaryContact { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+@[a-zA-Z0-9\u0080-\uFFA7?$#()""'!,+\-=_:;.&€£*%\s\/]+\.([a-zA-Z0-9\u0080-\uFFA7]{2,10})$", ErrorMessage = "Enter an email address in the correct format, like name@example.com")]
        public string SecondaryContactEmail { get; set; }

        [PhoneNumber(FieldName = "primary contact", IsRequired = false)]
        public string SecondaryContactPhone { get; set; }

        [Required(ErrorMessage = "You must tell us whether the provider should receive referrals")]
        public bool? IsEnabledForReferral { get; set; }

        public string SubmitAction { get; set; }
        public string Source { get; set; }
        public bool IsCdfProvider { get; set; }

        public IList<ProviderVenueViewModel> ProviderVenue { get; set; }

        public bool IsSaveSection=>
            !string.IsNullOrWhiteSpace(SubmitAction)
            && string.Equals(SubmitAction, "SaveSection", StringComparison.InvariantCultureIgnoreCase);
        public bool IsSaveAndFinish =>
            !string.IsNullOrWhiteSpace(SubmitAction)
            && string.Equals(SubmitAction, "SaveAndFinish", StringComparison.InvariantCultureIgnoreCase);

        public bool IsSaveAndAddVenue =>
            !string.IsNullOrWhiteSpace(SubmitAction)
            && string.Equals(SubmitAction, "SaveAndAddVenue", StringComparison.InvariantCultureIgnoreCase);
    }
}