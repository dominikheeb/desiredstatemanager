using System.Collections.Generic;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Chocolatey.Model;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Chocolatey.Model
{
    public class ChocolateyPackageTests
    {
        private ChocolateyPackage testChocolateyPackage;
        private ChocolateyPackage testDependency;
        private DscResourceDto resultDto;

        [Fact]
        public void TestChocolateyPackageToDto()
        {
            InitializeDependencyModel();
            GivenModelFilledWithValues();
            WhenConvertingToDto();
            ThenDtoContentIsCorrect();
        }

        private void ThenDtoContentIsCorrect()
        {
            Assert.IsType<ChocolateyPackageDto>(resultDto);
            var resultChocolateyPackageDto = (ChocolateyPackageDto)resultDto;
            Assert.Equal(testChocolateyPackage.ResourceStepName, resultChocolateyPackageDto.ResourceStepName);
            Assert.Equal(testChocolateyPackage.ChocolateyPackageName, resultChocolateyPackageDto.ChocolateyPackageName);
            Assert.Equal(testChocolateyPackage.ResourceName, resultChocolateyPackageDto.ResourceName);
            Assert.Equal(testChocolateyPackage.Ensure, resultChocolateyPackageDto.Ensure);
            Assert.Equal(testChocolateyPackage.DependsOn.Count, resultChocolateyPackageDto.DependsOn.Count);
            foreach (var dscDependency in resultChocolateyPackageDto.DependsOn)
            {
                //TODO: Decide if just string of Id's is usable too
                Assert.IsType<ChocolateyPackageDto>(dscDependency);
                Assert.Equal(testDependency.ResourceStepName, dscDependency.ResourceStepName);
            }
        }

        private void WhenConvertingToDto()
        {
            resultDto = testChocolateyPackage.ToResourceDto();
        }

        private void InitializeDependencyModel()
        {
            testDependency = new ChocolateyPackage
            {
                ChocolateyPackageName = "test",
                Ensure = Ensure.Absent,
                ResourceStepName = "testStep"
            };
        }

        private void GivenModelFilledWithValues()
        {
            testChocolateyPackage = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                Ensure = Ensure.Present,
                ResourceStepName = "dockerStep",
                DependsOn = new List<IDscResource> { testDependency }
            };
        }
    }
}