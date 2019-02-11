﻿using FluentAssertions;
using Microsoft.AspNetCore.Authorization;

using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.DataImport.Security
{
    public class When_Data_Import_Controller_Allow_Anonymous_Attribute
    {
        private AllowAnonymousAttribute[] _allowAnonymousAttributes;

        
        public void Setup()
        {
            var controllerType = typeof(DataImportController);

            _allowAnonymousAttributes = controllerType.GetCustomAttributes(typeof(AllowAnonymousAttribute), false)
                as AllowAnonymousAttribute[];
        }

        [Fact]
        public void Then_Is_Not_On_Controller() =>
            _allowAnonymousAttributes.Length.Should().Be(0);
    }
}