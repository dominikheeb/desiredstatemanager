using System.Collections.Generic;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Chocolatey.Model;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Chocolatey.Model
{
    public class ChocolateySourceTests
    {
        private ChocolateySource testChocolateySource;
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
            Assert.IsType<ChocolateySourceDto>(resultDto);
            var resultChocolateySourceDto = (ChocolateySourceDto)resultDto;
            Assert.Equal(testChocolateySource.ResourceStepName, resultChocolateySourceDto.ResourceStepName);
            Assert.Equal(testChocolateySource.ChocoPackageSource, resultChocolateySourceDto.ChocoPackageSource);
            Assert.Equal(testChocolateySource.ResourceName, resultChocolateySourceDto.ResourceName);
            Assert.Equal(testChocolateySource.Ensure, resultChocolateySourceDto.Ensure);
            Assert.Equal(testChocolateySource.DependsOn.Count, resultChocolateySourceDto.DependsOn.Count);
            foreach (var dscDependency in resultChocolateySourceDto.DependsOn)
            {
                //TODO: Decide if just string of Id's is usable too
                Assert.IsType<ChocolateyPackageDto>(dscDependency);
                Assert.Equal(testDependency.ResourceStepName, dscDependency.ResourceStepName);
            }
        }

        private void WhenConvertingToDto()
        {
            resultDto = testChocolateySource.ToResourceDto();
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
            testChocolateySource = new ChocolateySource
            {
                ChocoPackageSource = "https://test123",
                Ensure = Ensure.Present,
                ResourceStepName = "dockerStep",
                DependsOn = new List<IDscResource> { testDependency }
            };
        }
    }
}