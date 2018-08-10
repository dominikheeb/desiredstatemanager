using System.Collections.Generic;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Core
{
    public class DscResourceDtoTests
    {
        private DscResourceDto resourceDto;
        private string createdString;

        [Fact]
        public void TestToStringToDscFormat()
        {
            GivenDscResourceContainsInformations();
            WhenUsingToString();
            ThenDscFormatIsCreated();
        }

        private void ThenDscFormatIsCreated()
        {
            Assert.NotNull(createdString);
            Assert.Equal($"[{resourceDto.ResourceName}]{resourceDto.ResourceStepName}", createdString);
        }

        private void WhenUsingToString()
        {
            createdString = resourceDto.ToString();
        }

        private void GivenDscResourceContainsInformations()
        {
            var dscDependency = new ChocolateyPackageDto
            {
                ChocolateyPackageName = "Dependency",
                DependsOn = new List<DscResourceDto>(),
                Ensure = Ensure.Present,
                ResourceName = "TestResource",
                ResourceStepName = "TestStep"
            };

            resourceDto = new ChocolateyPackageDto
            {
                ChocolateyPackageName = "TestPackage",
                DependsOn = new List<DscResourceDto> {dscDependency},
                Ensure = Ensure.Absent,
                ResourceName = "TestMainResource",
                ResourceStepName = "TestMainStep"
            };
        }
    }
}