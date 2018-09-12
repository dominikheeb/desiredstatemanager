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
        private MergeResult<IDscResource> mergeResultGiven;
        private ChocolateyPackage modelGivenForMerge;
        private List<MergeResult<IDscResource>> mergedResult;

        [Fact]
        public void TestChocolateyPackageToDto()
        {
            InitializeDependencyModel();
            GivenModelFilledWithValues();
            WhenConvertingToDto();
            ThenDtoContentIsCorrect();
        }

        [Fact]
        public void TestChocolateyPackageToDtoWithoutDependency()
        {
            GivenModelFilledWithValuesWithoutDependency();
            WhenConvertingToDto();
            ThenDtoContentIsCorrectWithoutDependencies();
        }

        [Fact]
        public void TestChocolateyPackageMerge()
        {
            GivenMergeResultAndModelWithSameNameAndType();
            WhenMergin();
            ThenMergeResultIsOverridden();
        }

        [Fact]
        public void TestChocolateyPackageMergeOnPresentAbsent()
        {
            GivenMergeResultAndModelWithSamePackageButDifferentEnsure();
            WhenMergin();
            ThenMergeResultIsOverridden();
        }

        [Fact]
        public void TestChocolateyPackageMergeWithSamePackage()
        {
            GivenMergeResultAndModelWithSamePackage();
            WhenMergin();
            ThenMergeResultIsOverridden();
        }

        private void ThenMergeResultIsOverridden()
        {
            var mergeResult = Assert.Single(mergedResult);
            Assert.NotNull(mergeResult);
            Assert.True(mergeResult.Success);
            var chocolateySource = Assert.IsType<ChocolateyPackage>(mergeResult.Value);
            Assert.Equal(modelGivenForMerge.ResourceStepName, chocolateySource.ResourceStepName);
            Assert.Equal(modelGivenForMerge.ChocolateyPackageName, chocolateySource.ChocolateyPackageName);
            Assert.Equal(modelGivenForMerge.ChocolateyPackageVersion, chocolateySource.ChocolateyPackageVersion);
            Assert.Equal(modelGivenForMerge.ResourceName, chocolateySource.ResourceName);
            Assert.Equal(modelGivenForMerge.Ensure, chocolateySource.Ensure);
        }

        private void ThenDtoContentIsCorrectWithoutDependencies()
        {
            Assert.IsType<ChocolateyPackageDto>(resultDto);
            var resultChocolateyPackageDto = (ChocolateyPackageDto)resultDto;
            Assert.Equal(testChocolateyPackage.ResourceStepName, resultChocolateyPackageDto.ResourceStepName);
            Assert.Equal(testChocolateyPackage.ChocolateyPackageName, resultChocolateyPackageDto.ChocolateyPackageName);
            Assert.Equal(testChocolateyPackage.ResourceName, resultChocolateyPackageDto.ResourceName);
            Assert.Equal(testChocolateyPackage.Ensure, resultChocolateyPackageDto.Ensure);
            Assert.Equal(testChocolateyPackage.ChocolateyPackageVersion, resultChocolateyPackageDto.ChocolateyPackageVersion);
            Assert.Null(resultChocolateyPackageDto.DependsOn);
        }

        private void ThenDtoContentIsCorrect()
        {
            Assert.IsType<ChocolateyPackageDto>(resultDto);
            var resultChocolateyPackageDto = (ChocolateyPackageDto)resultDto;
            Assert.Equal(testChocolateyPackage.ResourceStepName, resultChocolateyPackageDto.ResourceStepName);
            Assert.Equal(testChocolateyPackage.ChocolateyPackageName, resultChocolateyPackageDto.ChocolateyPackageName);
            Assert.Equal(testChocolateyPackage.ResourceName, resultChocolateyPackageDto.ResourceName);
            Assert.Equal(testChocolateyPackage.Ensure, resultChocolateyPackageDto.Ensure);
            Assert.Equal(testChocolateyPackage.ChocolateyPackageVersion, resultChocolateyPackageDto.ChocolateyPackageVersion);
            Assert.Equal(testChocolateyPackage.DependsOn.Count, resultChocolateyPackageDto.DependsOn.Count);
            foreach (var dscDependency in resultChocolateyPackageDto.DependsOn)
            {
                //TODO: Decide if just string of Id's is usable too
                Assert.IsType<ChocolateyPackageDto>(dscDependency);
                Assert.Equal(testDependency.ResourceStepName, dscDependency.ResourceStepName);
            }
        }

        private void WhenMergin()
        {
            mergedResult = modelGivenForMerge.MergeDscResources(new List<MergeResult<IDscResource>> {mergeResultGiven});
        }

        private void WhenConvertingToDto()
        {
            resultDto = testChocolateyPackage.ToResourceDto();
        }

        private void GivenMergeResultAndModelWithSamePackage()
        {
            mergeResultGiven = new MergeResult<IDscResource>
            {
                Value = new ChocolateyPackage
                {
                    ResourceStepName = "TestStep",
                    ChocolateyPackageName = "PackageTest",
                    ChocolateyPackageVersion = "1.2",
                    Ensure = Ensure.Present
                }
            };

            modelGivenForMerge = new ChocolateyPackage
            {
                ResourceStepName = "TestStep2",
                ChocolateyPackageName = "PackageTest",
                ChocolateyPackageVersion = "1.1",
                Ensure = Ensure.Present
            };
        }

        private void GivenMergeResultAndModelWithSamePackageButDifferentEnsure()
        {
            mergeResultGiven = new MergeResult<IDscResource>
            {
                Value = new ChocolateyPackage
                {
                    ResourceStepName = "TestStep",
                    ChocolateyPackageName = "PackageTest",
                    ChocolateyPackageVersion = "1.2",
                    Ensure = Ensure.Present
                }
            };

            modelGivenForMerge = new ChocolateyPackage
            {
                ResourceStepName = "TestStep2",
                ChocolateyPackageName = "PackageTest",
                ChocolateyPackageVersion = "1.3",
                Ensure = Ensure.Absent
            };
        }

        private void GivenMergeResultAndModelWithSameNameAndType()
        {
            mergeResultGiven = new MergeResult<IDscResource>
            {
                Value = new ChocolateyPackage
                {
                    ResourceStepName = "TestStep",
                    ChocolateyPackageName = "PackageTest",
                    ChocolateyPackageVersion = "1.2",
                    Ensure = Ensure.Present
                }
            };

            modelGivenForMerge = new ChocolateyPackage
            {
                ResourceStepName = "TestStep",
                ChocolateyPackageName = "PackageTest",
                ChocolateyPackageVersion = "1.3",
                Ensure = Ensure.Present
            };
        }

        private void GivenModelFilledWithValuesWithoutDependency()
        {
            testChocolateyPackage = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                Ensure = Ensure.Present,
                ResourceStepName = "dockerStep"
            };
        }

        private void GivenModelFilledWithValues()
        {
            testChocolateyPackage = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                Ensure = Ensure.Present,
                ResourceStepName = "dockerStep",
                DependsOn = new List<IDscResource> { testDependency },
                ChocolateyPackageVersion = "1.23.4"
            };
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
    }
}