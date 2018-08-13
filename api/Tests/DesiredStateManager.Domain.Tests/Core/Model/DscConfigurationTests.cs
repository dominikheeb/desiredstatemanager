using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Chocolatey.Model;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Core.Model
{
    public class DscConfigurationTests
    {
        private ChocolateyPackage dockerChocolateyResource;
        private ChocolateyPackage visualStudioChocolateyResource;
        private ChocolateySource chocolateySourceResource;
        private DscConfiguration testDscConfiguration;
        private DscConfigurationDto resultDto;
        
        [Fact]
        public void TestDscConfigurationToDto()
        {
            InitializeTestDscResources();
            GivenDscConfigurationWithResources();
            WhenUsingToDto();
            ThenDtoIsCreatedWithCorrectResourceDtos();
        }

        [Fact]
        public void TestDscConfigurationToDtoWithoutResources()
        {
            GivenDscConfigurationWithoutResources();
            WhenUsingToDto();
            ThenDtoIsCreatedWithoutDscResources();
        }

        private void ThenDtoIsCreatedWithoutDscResources()
        {
            Assert.Null(testDscConfiguration.DscResources);
        }

        private void GivenDscConfigurationWithoutResources()
        {
            testDscConfiguration = new DscConfiguration();
        }

        private void ThenDtoIsCreatedWithCorrectResourceDtos()
        {
            Assert.Equal(3, resultDto.DscResourceDtos.Count);
            var chocoSourceDto = resultDto.DscResourceDtos.Single(x => x.ResourceStepName.Equals("chocoSource"));
            Assert.IsType<ChocolateySourceDto>(chocoSourceDto);

            var dockerResourceDto = resultDto.DscResourceDtos.Single(x => x.ResourceStepName.Equals("dockerStep"));
            Assert.IsType<ChocolateyPackageDto>(dockerResourceDto);

            var visualStudioResourceDto = resultDto.DscResourceDtos.Single(x => x.ResourceStepName.Equals("visualStudioStep"));
            Assert.IsType<ChocolateyPackageDto>(visualStudioResourceDto);
        }

        private void WhenUsingToDto()
        {
            resultDto = testDscConfiguration.ToConfigurationDto();
        }

        private void InitializeTestDscResources()
        {
            chocolateySourceResource = new ChocolateySource
            {
                Ensure = Ensure.Present,
                ResourceStepName = "chocoSource",
                ChocoPackageSource = "https://chocolateySource"
            };

            dockerChocolateyResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                ResourceStepName = "dockerStep",
                DependsOn = new List<IDscResource> { chocolateySourceResource },
                Ensure = Ensure.Present
            };

            visualStudioChocolateyResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "visualstudio",
                Ensure = Ensure.Present,
                DependsOn = new List<IDscResource> { dockerChocolateyResource },
                ResourceStepName = "visualStudioStep"
            };
        }

        private void GivenDscConfigurationWithResources()
        {
            testDscConfiguration = new DscConfiguration
            {
                DscResources = new List<IDscResource>
                {
                    dockerChocolateyResource,
                    chocolateySourceResource,
                    visualStudioChocolateyResource
                }
            };
        }
    }
}