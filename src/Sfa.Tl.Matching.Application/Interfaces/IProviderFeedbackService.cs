﻿using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderFeedbackService
    {
        Task RequestProviderQuarterlyUpdateAsync(string userName);
        Task<int> SendProviderQuarterlyUpdateEmailsAsync(int backgroundProcessHistoryId, string userName);
    }
}