﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using SFA.DAS.Notifications.Api.Client;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralService_Is_Called_To_Send_ProvisionGap_Email
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly INotificationsApi _notificationsApi;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailPlaceholder> _emailPlaceholderRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IRepository<Domain.Models.Opportunity> _opportunityRepository;

        public When_ReferralService_Is_Called_To_Send_ProvisionGap_Email()
        {
            _configuration = new MatchingConfiguration();
            _notificationsApi = Substitute.For<INotificationsApi>();
            _emailService = Substitute.For<IEmailService>();

            var logger = Substitute.For<ILogger<ReferralService>>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            _emailHistoryRepository = Substitute.For<IRepository<EmailHistory>>();
            _emailPlaceholderRepository = Substitute.For<IRepository<EmailPlaceholder>>();
            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            _opportunityRepository = Substitute.For<IRepository<Domain.Models.Opportunity>>();

            _emailTemplateRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.EmailTemplate, bool>>>())
                .Returns(new ValidEmailTemplateBuilder().Build());

            _opportunityRepository
                .GetSingleOrDefault(
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(), 
                    Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>[]>())
                .Returns(new ValidOpportunityWithProvisionGapBuilder().Build());

            var referralService = new ReferralService(_emailService, 
                _emailHistoryRepository,_emailPlaceholderRepository,_emailTemplateRepository, _opportunityRepository,
                mapper, logger);

            referralService.SendProvisionGapEmail(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<dynamic>(), Arg.Any<string>());
        }
        
        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Any<Domain.Models.EmailHistory>());
        }

        //[Fact]
        //public void Then_EmailPlaceholderRepository_CreateMany_Is_Called_Exactly_Once()
        //{
        //    _emailPlaceholderRepository
        //        .Received(1)
        //        .CreateMany(Arg.Any<IList<Domain.Models.EmailPlaceholder>>());
        //}
        
        [Fact]
        public void Then_OpportunityRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetSingleOrDefault(
                Arg.Any<Expression<Func<Domain.Models.Opportunity, bool>>>(),
                Arg.Any<Expression<Func<Domain.Models.Opportunity, object>>[]>());
        }
    }
}
