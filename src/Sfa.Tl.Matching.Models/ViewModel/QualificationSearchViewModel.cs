﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sfa.Tl.Matching.Models.ViewModel
{
    public class QualificationSearchViewModel
    {
        [Required(ErrorMessage = "You must enter 2 or more letters for your search")]
        [MinLength(2, ErrorMessage = "You must enter 2 or more letters for your search")]
        [RegularExpression(@"^(?!^\d+$)^.+$", ErrorMessage = "You must enter 2 or more letters for your search")]
        public string Title { get; set; }
        public IList<QualificationSearchResultViewModel> Results { get; set; } = new List<QualificationSearchResultViewModel>();
        public int ResultCount { get; set; }
    }
}