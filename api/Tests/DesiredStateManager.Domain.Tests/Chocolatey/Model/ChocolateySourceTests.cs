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
        private MergeResult<DscResource> mergeResultGiven;
        private ChocolateySource modelGivenForMerge;
        private List<MergeResult<DscResource>> mergedResult;

        [Fact]
        public void TestChocolateySourceToDto()
        {
            InitializeDependencyModel();
            GivenModelFilledWithValues();
            WhenConvertingToDto();
            ThenDtoContentIsCorrect();
        }

        [Fact]
        public void TestChocolateySourceToDtoWithoutDependencies()
        {
            GivenModelFilledWithValuesWithoutDependencies();
            WhenConvertingToDto();
            ThenDtoContentIsCorrectWithoutDependencies();
        }

        [Fact]
        public void TestChocolateySourceMerge()
        {
            GivenMergeResultAndModelWithSameNameAndType();
            WhenMergin();
            ThenMergeResultIsOverridden();
        }

        [Fact]
        public void TestChocolateySourceMergeOnPresentAbsent()
        {
            GivenMergeResultAndModelWithSameSourceButDifferentEnsure();
            WhenMergin();
            ThenMergeResultIsOverridden();
        }

        [Fact]
        public void TestChocolateySourceMergeWithSameSource()
        {
            GivenMergeResultAndModelWithSameSource();
            WhenMergin();
            ThenMergeResultIsOverridden();
        }

        private void GivenMergeResultAndModelWithSameSource()
        {
            mergeResultGiven = new MergeResult<DscResource>
            {
                Value = new ChocolateySource
                {
                    ResourceStepName = "TestStep",
                    ChocoPackageSource = "Test.ch",
                    Ensure = Ensure.Present
                }
            };

            modelGivenForMerge = new ChocolateySource
            {
                ResourceStepName = "Step1",
                ChocoPackageSource = "Test.ch",
                Ensure = Ensure.Present
            };
        }

        private void GivenMergeResultAndModelWithSameSourceButDifferentEnsure()
        {
            mergeResultGiven = new MergeResult<DscResource>
            {
                Value = new ChocolateySource
                {
                    ResourceStepName = "TestStep",
                    ChocoPackageSource = "Test.ch",
                    Ensure = Ensure.Present
                }
            };

            modelGivenForMerge = new ChocolateySource
            {
                ResourceStepName = "Step2",
                ChocoPackageSource = "Test.ch",
                Ensure = Ensure.Absent
            };
        }

        private void ThenMergeResultIsOverridden()
        {
            var mergeResult = Assert.Single(mergedResult);
            Assert.NotNull(mergeResult);
            Assert.True(mergeResult.Success);
            var chocolateySource = Assert.IsType<ChocolateySource>(mergeResult.Value);
            Assert.Equal(modelGivenForMerge.ResourceStepName, chocolateySource.ResourceStepName);
            Assert.Equal(modelGivenForMerge.ChocoPackageSource, chocolateySource.ChocoPackageSource);
            Assert.Equal(modelGivenForMerge.ResourceName, chocolateySource.ResourceName);
            Assert.Equal(modelGivenForMerge.Ensure, chocolateySource.Ensure);
        }

        private void WhenMergin()
        {
            mergedResult = modelGivenForMerge.MergeDscResources(new List<MergeResult<DscResource>> {mergeResultGiven});
        }

        private void GivenMergeResultAndModelWithSameNameAndType()
        {
            mergeResultGiven = new MergeResult<DscResource>
            {
                Value = new ChocolateySource
                {
                    ResourceStepName = "TestStep",
                    ChocoPackageSource = "Test.ch",
                    Ensure = Ensure.Present
                }
            };

            modelGivenForMerge = new ChocolateySource
            {
                ResourceStepName = "TestStep",
                ChocoPackageSource = "Test.de",
                Ensure = Ensure.Present
            };
        }

        private void ThenDtoContentIsCorrectWithoutDependencies()
        {
            Assert.IsType<ChocolateySourceDto>(resultDto);
            var resultChocolateySourceDto = (ChocolateySourceDto)resultDto;
            Assert.Equal(testChocolateySource.ResourceStepName, resultChocolateySourceDto.ResourceStepName);
            Assert.Equal(testChocolateySource.ChocoPackageSource, resultChocolateySourceDto.ChocoPackageSource);
            Assert.Equal(testChocolateySource.ResourceName, resultChocolateySourceDto.ResourceName);
            Assert.Equal(testChocolateySource.Ensure, resultChocolateySourceDto.Ensure);
            Assert.Null(resultChocolateySourceDto.DependsOn);
        }

        private void GivenModelFilledWithValuesWithoutDependencies()
        {
            testChocolateySource = new ChocolateySource
            {
                ChocoPackageSource = "https://test123",
                Ensure = Ensure.Present,
                ResourceStepName = "dockerStep"
            };
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
                DependsOn = new List<DscResource> { testDependency }
            };
        }
    }
}