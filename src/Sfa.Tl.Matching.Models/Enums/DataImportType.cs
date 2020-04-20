﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Models.Enums
{
    public enum DataImportType
    {
        [Description("Learning Aim Reference")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Csv)]
        LearningAimReference,

        [Description("ONS Postcodes")]
        [DisplayName("Local Enterprise Partnership")]
        [FileExtensions(Extensions = FileImportTypeExtensions.Zip)]
        OnsPostcodes
    }
}