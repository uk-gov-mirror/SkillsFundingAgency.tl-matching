﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Humanizer;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.InMemoryDb;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common.AutoDomain;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ReferralEmail
{
    public class When_ReferralEmailService_Is_Called_To_Send_Employer_Email_Using_InMemory_Db
    {

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_To_Employers(
                        MatchingDbContext dbContext,
                        IDateTimeProvider dateTimeProvider,
                        MapperConfiguration mapperConfiguration,
                        [Frozen] Domain.Models.Opportunity opportunity,
                        [Frozen] Domain.Models.Provider provider,
                        [Frozen] Domain.Models.ProviderVenue venue,
                        [Frozen] BackgroundProcessHistory backgroundProcessHistory,
                        [Frozen] IEmailService emailService,
                        ILogger<OpportunityRepository> logger,
                        ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
                        ILogger<GenericRepository<OpportunityItem>> itemLogger,
                        ILogger<GenericRepository<FunctionLog>> functionLogLogger
                        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var mapper = new Mapper(mapperConfiguration);
            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);

            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendEmployerReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Placements_List(
            MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MapperConfiguration mapperConfiguration,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var mapper = new Mapper(mapperConfiguration);
            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);
            
            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            var itemIds = itemRepo.GetManyAsync(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var expectedResult = await GetExpectedResult(repo, opportunity.Id, itemIds);

            //Act
            await sut.SendEmployerReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("placements_list")), Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("placements_list") && tokens["placements_list"] == expectedResult), 
                Arg.Any<string>());

        }

        [Theory, AutoDomainData]
        public async Task Then_Send_Email_Is_Called_With_Employer_Details(
                            MatchingDbContext dbContext,
                            IDateTimeProvider dateTimeProvider,
                            MapperConfiguration mapperConfiguration,
                            [Frozen] Domain.Models.Opportunity opportunity,
                            [Frozen] Domain.Models.Provider provider,
                            [Frozen] Domain.Models.ProviderVenue venue,
                            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
                            [Frozen] IEmailService emailService,
                            ILogger<OpportunityRepository> logger,
                            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
                            ILogger<GenericRepository<OpportunityItem>> itemLogger,
                            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var mapper = new Mapper(mapperConfiguration);
            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);
            
            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendEmployerReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Any<IDictionary<string, string>>(), 
                Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_name") && tokens["employer_contact_name"] == opportunity.EmployerContact.ToTitleCase()), Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_business_name") && tokens["employer_business_name"] == opportunity.Employer.CompanyName.ToTitleCase()), 
                Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("employer_contact_number") && tokens["employer_contact_number"] == opportunity.EmployerContactPhone), 
                Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens => 
                    tokens.ContainsKey("employer_contact_email") && tokens["employer_contact_email"] == opportunity.EmployerContactEmail), 
                Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Placements_List_Should_Be_Empty_If_Opp_Items_Not_Saved(
            MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MapperConfiguration mapperConfiguration,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory, false, false);

            var mapper = new Mapper(mapperConfiguration);
            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);
            
            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            var itemIds = itemRepo.GetManyAsync(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var expectedResult = await GetExpectedResult(repo, opportunity.Id, itemIds);

            //Act
            await sut.SendEmployerReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("placements_list") && tokens["placements_list"] == string.Empty), 
                Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens => tokens.ContainsKey("placements_list") && tokens["placements_list"] == expectedResult), 
                Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Placements_List_Should_Be_Empty_If_Not_Selected_For_Referral(
            MatchingDbContext dbContext,
            IDateTimeProvider dateTimeProvider,
            MapperConfiguration mapperConfiguration,
            [Frozen] Domain.Models.Opportunity opportunity,
            [Frozen] Domain.Models.Provider provider,
            [Frozen] Domain.Models.ProviderVenue venue,
            [Frozen] BackgroundProcessHistory backgroundProcessHistory,
            [Frozen] IEmailService emailService,
            ILogger<OpportunityRepository> logger,
            ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
            ILogger<GenericRepository<OpportunityItem>> itemLogger,
            ILogger<GenericRepository<FunctionLog>> functionLogLogger
        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory, false, false);

            var mapper = new Mapper(mapperConfiguration);
            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);
            
            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            var itemIds = itemRepo.GetManyAsync(oi => oi.Opportunity.Id == opportunity.Id
                                                 && oi.IsSaved
                                                 && oi.IsSelectedForReferral
                                                 && !oi.IsCompleted).Select(oi => oi.Id);

            var expectedResult = await GetExpectedResult(repo, opportunity.Id, itemIds);

            //Act
            await sut.SendEmployerReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("placements_list") && tokens["placements_list"] == string.Empty), 
                Arg.Any<string>());

            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), 
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("placements_list") && tokens["placements_list"] == expectedResult), 
                Arg.Any<string>());
        }

        [Theory, AutoDomainData]
        public async Task Then_Background_Process_History_Status_Is_Completed(
                        MatchingDbContext dbContext,
                        IDateTimeProvider dateTimeProvider,
                        MapperConfiguration mapperConfiguration,
                        [Frozen] Domain.Models.Opportunity opportunity,
                        [Frozen] Domain.Models.Provider provider,
                        [Frozen] Domain.Models.ProviderVenue venue,
                        [Frozen] BackgroundProcessHistory backgroundProcessHistory,
                        [Frozen] IEmailService emailService,
                        ILogger<OpportunityRepository> logger,
                        ILogger<GenericRepository<BackgroundProcessHistory>> historyLogger,
                        ILogger<GenericRepository<OpportunityItem>> itemLogger,
                        ILogger<GenericRepository<FunctionLog>> functionLogLogger
                        )
        {
            //Arrange
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Pending.ToString();

            await DataBuilder.SetTestData(dbContext, provider, venue, opportunity, backgroundProcessHistory);

            var mapper = new Mapper(mapperConfiguration);
            var repo = new OpportunityRepository(logger, dbContext);
            var backgroundRepo = new GenericRepository<BackgroundProcessHistory>(historyLogger, dbContext);
            var itemRepo = new GenericRepository<OpportunityItem>(itemLogger, dbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(functionLogLogger, dbContext);

            var sut = new ReferralEmailService(mapper, dateTimeProvider, emailService, repo, itemRepo, backgroundRepo, functionLogRepository);

            //Act
            await sut.SendEmployerReferralEmailAsync(opportunity.Id, opportunity.OpportunityItem.Select(oi => oi.Id), backgroundProcessHistory.Id, "System");

            //Assert
            await emailService.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int?>(), Arg.Any<int?>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());

            var processStatus = dbContext.BackgroundProcessHistory
                .FirstOrDefault(history => history.Id == backgroundProcessHistory.Id)
                ?.Status;

            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Pending.ToString());
            processStatus.Should().NotBe(BackgroundProcessHistoryStatus.Processing.ToString());
            processStatus.Should().Be(BackgroundProcessHistoryStatus.Complete.ToString());
        }

        private static async Task<string> GetExpectedResult(OpportunityRepository repo, int opportunityId, IEnumerable<int> itemIds)
        {
            var employerReferral = await repo.GetEmployerReferralsAsync(opportunityId, itemIds);
            var sb = new StringBuilder();

            foreach (var data in employerReferral.WorkplaceDetails.OrderBy(dto => dto.WorkplaceTown))
            {
                var placements = GetNumberOfPlacements(data.PlacementsKnown, data.Placements);

                sb.AppendLine($"# {data.WorkplaceTown} {data.WorkplacePostcode}");
                sb.AppendLine($"* Job role: {data.JobRole}");
                sb.AppendLine($"* Students wanted: {placements}");

                var count = 1;
                foreach (var providerAndVenue in data.ProviderAndVenueDetails)
                {
                    sb.AppendLine($"* {count.ToOrdinalWords().ToTitleCase()} provider selected: {providerAndVenue.CustomisedProviderDisplayName}");
                    sb.AppendLine($"Primary contact: {providerAndVenue.PrimaryContact} (Telephone: {providerAndVenue.PrimaryContactPhone}; Email: {providerAndVenue.PrimaryContactEmail})");
                    sb.AppendLine($"Secondary contact: {providerAndVenue.SecondaryContact} (Telephone: {providerAndVenue.SecondaryContactPhone}; Email: {providerAndVenue.SecondaryContactEmail})");
                    count++;
                }

                sb.AppendLine("");
            }

            return sb.ToString();
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least 1";
        }
    }
}