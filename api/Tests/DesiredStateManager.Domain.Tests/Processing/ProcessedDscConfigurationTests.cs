using System.Collections.Generic;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Processing;
using DesiredStateManager.Domain.Processing.Model;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Processing
{
    public class ProcessedDscConfigurationTests
    {
        private ChocolateyPackageDto dockerChocolateyResourceDto;
        private ChocolateyPackageDto visualStudioChocolateyResourceDto;
        private DscConfigurationDto dscConfigurationDto;
        private ProcessedDscConfiguration processedDscConfiguration;
        private ChocolateyPackageDto resharperChocolateyResourceDto;

        [Fact]
        public void TestDscConfigurationProcessing()
        {
            InitalizeTestDtos();
            GivenDscConfigurationDtoWithChocolateyResourcesInWrongOrder();
            WhenProcessingConfigurationDto();
            ThenProcessedDscConfigurationIsCreatedWithCorrectOrder();
        }

        private void ThenProcessedDscConfigurationIsCreatedWithCorrectOrder()
        {
            Assert.NotNull(processedDscConfiguration);
            Assert.NotNull(processedDscConfiguration.ProcessedDscResources);
            Assert.Equal(3, processedDscConfiguration.ProcessedDscResources.Count);

            //Test first Resource is Docker since visual studio depends on the docker resource
            var firstProcessedResource = processedDscConfiguration.ProcessedDscResources[0];
            Assert.Equal(dockerChocolateyResourceDto.ResourceName, firstProcessedResource.ResourceName);
            Assert.Equal(dockerChocolateyResourceDto.ResourceStepName, firstProcessedResource.ResourceStepName);

            Assert.Contains("Ensure", firstProcessedResource.DscProperties.Keys);
            Assert.Equal("Present", firstProcessedResource.DscProperties["Ensure"]);
            Assert.DoesNotContain("DependsOn", firstProcessedResource.DscProperties.Keys);
            Assert.Contains("Name", firstProcessedResource.DscProperties.Keys);
            Assert.Equal(dockerChocolateyResourceDto.ChocolateyPackageName, firstProcessedResource.DscProperties["Name"]);

            var secondProcessedDscResource = processedDscConfiguration.ProcessedDscResources[1];
            Assert.Equal(visualStudioChocolateyResourceDto.ResourceName, secondProcessedDscResource.ResourceName);
            Assert.Equal(visualStudioChocolateyResourceDto.ResourceStepName, secondProcessedDscResource.ResourceStepName);

            Assert.Contains("Ensure", secondProcessedDscResource.DscProperties.Keys);
            Assert.Equal("Present", secondProcessedDscResource.DscProperties["Ensure"]);
            Assert.Contains("DependsOn", secondProcessedDscResource.DscProperties.Keys);
            Assert.Equal("[cChocoPackageInstaller]dockerStep",secondProcessedDscResource.DscProperties["DependsOn"]);
            Assert.Contains("Name", secondProcessedDscResource.DscProperties.Keys);
            Assert.Equal(visualStudioChocolateyResourceDto.ChocolateyPackageName, secondProcessedDscResource.DscProperties["Name"]);

            var thirdProcessedDscResource = processedDscConfiguration.ProcessedDscResources[2];
            Assert.Equal(resharperChocolateyResourceDto.ResourceName, thirdProcessedDscResource.ResourceName);
            Assert.Equal(resharperChocolateyResourceDto.ResourceStepName, thirdProcessedDscResource.ResourceStepName);

            Assert.Contains("Ensure", thirdProcessedDscResource.DscProperties.Keys);
            Assert.Equal("Present", thirdProcessedDscResource.DscProperties["Ensure"]);
            Assert.Contains("DependsOn", thirdProcessedDscResource.DscProperties.Keys);
            Assert.Equal($"@([{dockerChocolateyResourceDto.ResourceName}]{dockerChocolateyResourceDto.ResourceStepName}, [{visualStudioChocolateyResourceDto.ResourceName}]{visualStudioChocolateyResourceDto.ResourceStepName})", thirdProcessedDscResource.DscProperties["DependsOn"]);
            Assert.Contains("Name", thirdProcessedDscResource.DscProperties.Keys);
            Assert.Equal(resharperChocolateyResourceDto.ChocolateyPackageName, thirdProcessedDscResource.DscProperties["Name"]);
        }

        private void WhenProcessingConfigurationDto()
        {
            processedDscConfiguration = ProcessedDscConfiguration.FromDscConfiguration(dscConfigurationDto);
        }

        private void InitalizeTestDtos()
        {
            dockerChocolateyResourceDto = new ChocolateyPackageDto
            {
                ResourceName = "cChocoPackageInstaller",
                ChocolateyPackageName = "docker-for-windows",
                ResourceStepName = "dockerStep",
                Ensure = Ensure.Present
            };

            visualStudioChocolateyResourceDto = new ChocolateyPackageDto
            {
                ResourceName = "cChocoPackageInstaller",
                ChocolateyPackageName = "visualstudio",
                Ensure = Ensure.Present,
                DependsOn = new List<DscResourceDto> { dockerChocolateyResourceDto },
                ResourceStepName = "visualStudioStep"
            };

            resharperChocolateyResourceDto = new ChocolateyPackageDto
            {
                ResourceName = "cChocoPackageInstaller",
                ChocolateyPackageName = "resharper",
                Ensure = Ensure.Present,
                DependsOn = new List<DscResourceDto> { dockerChocolateyResourceDto, visualStudioChocolateyResourceDto },
                ResourceStepName = "resharperStep"
            };
        }

        private void GivenDscConfigurationDtoWithChocolateyResourcesInWrongOrder()
        {
            dscConfigurationDto = new DscConfigurationDto();
            dscConfigurationDto.DscResourceDtos.Add(visualStudioChocolateyResourceDto);
            dscConfigurationDto.DscResourceDtos.Add(dockerChocolateyResourceDto);
            dscConfigurationDto.DscResourceDtos.Add(resharperChocolateyResourceDto);
        }

        
    }
}
