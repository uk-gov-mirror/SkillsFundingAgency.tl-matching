﻿using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces.FeedbackFactory;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.Services.FeedbackFactory;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.FeedbackEmails
{
    public class When_SendProviderFeedbackEmails_Function_Timer_Trigger_Fires
    {
        private readonly IFeedbackServiceFactory<ProviderFeedbackService> _providerFeedbackService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_SendProviderFeedbackEmails_Function_Timer_Trigger_Fires()
        {
            var timerSchedule = Substitute.For<TimerSchedule>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            _providerFeedbackService = Substitute.For<IFeedbackServiceFactory<ProviderFeedbackService>>();

            var providerFeedback = new Functions.ProviderFeedback();
            providerFeedback.SendProviderFeedbackEmails(
                new TimerInfo(timerSchedule, new ScheduleStatus()),
                new ExecutionContext(),
                new NullLogger<Functions.ProviderFeedback>(),
                _providerFeedbackService,
                _functionLogRepository).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void SendFeedbackEmailsAsync_Is_Called_Exactly_Once()
        {
            _providerFeedbackService
                .Received(1)
                .CreateInstanceOf(FeedbackEmailTypes.ProviderFeedback).SendFeedbackEmailsAsync(
                    Arg.Is<string>(x => x == "System"));
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .Create(Arg.Any<FunctionLog>());
        }
    }
}