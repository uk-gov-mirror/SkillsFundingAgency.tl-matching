﻿using System;
using System.Collections.Generic;

namespace Sfa.Tl.Matching.Data.Models
{
    public class TemplatePlaceholder
    {
        public Guid Id { get; set; }
        public Guid EmailTemplateId { get; set; }
        public string Placeholder { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual EmailTemplate EmailTemplate { get; set; }
    }
}
