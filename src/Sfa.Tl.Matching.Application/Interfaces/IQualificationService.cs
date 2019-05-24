﻿using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IQualificationService
    {
        Task<int> CreateQualificationAsync(AddQualificationViewModel viewModel);
    }
}
