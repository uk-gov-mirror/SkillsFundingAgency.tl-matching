﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Api.Client.Configuration;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Web.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
        }

        protected override void ConfigureConfiguration(IServiceCollection services)
        {
            MatchingConfiguration = new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://postcodes.io/postcodes",
                NotificationsApiClientConfiguration = new NotificationsApiClientConfiguration
                {
                    ApiBaseUrl = "http://localhost:56133/"
                }
            };
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
        }
    }
}