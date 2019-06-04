﻿using System.ComponentModel;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.UkRlp.Api.Client
{
    public enum UkRlpRecordStatus
    {
        [Description("A")]
        Active,
        [Description("V")]
        Verified,
        [Description("PD1")]
        DeactivationInProcess,
        [Description("PD2")]
        DeactivateComplete
    }
}