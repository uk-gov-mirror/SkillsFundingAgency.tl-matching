﻿using Sfa.Tl.Matching.Application.FileReader.ProviderVenueQualification;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueQualificationReader
    {
        ProviderVenueQualificationReadResult ReadData(ProviderVenueQualificationFileImportDto fileImportDto);
    }
}
