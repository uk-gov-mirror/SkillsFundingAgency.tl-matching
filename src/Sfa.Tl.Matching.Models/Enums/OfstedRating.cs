﻿using System.ComponentModel;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum OfstedRating
    {
        Outstanding = 1,
        Good,
        [Description("Requires improvement")]
        RequiresImprovement,
        Inadequate,
        NotApplicable
    }
}