using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Core.Model
{
    public partial class MergedPreferenceTests
    {
        private MockDscResource chocolateySourceResource;
        private MockDscResource dockerChocolateyResource;
        private MockDscResource2 visualStudioChocolateyResource;
        private MockDscResource absentDockerChocolateyResource;
        private MockDscResource firefoxChocolateyResource;
        private ProjectPreference projectPreference;
        private UserPreference userPreference;
        private MergedPreference resultPreference;
        private MergedPreference mergeResult;

        [Fact]
        public void TestMergeEmptyPrefernenceMergeResult()
        {
            GivenUserPreference();
            WhenMergeUserPreferenceWithEmptyPreference();
            ThenUserPreferenceIsApplied();
        }

        [Fact]
        public void TestPreferenceMerge()
        {
            InitializeTestDscResources();
            GivenUserAndProjectPreferences();
            WhenMergePreferences();
            ThenCorrectResourcesAreCreated();
        }

        [Fact]
        public void TestMergeStepNaming()
        {
            GivenUserAndProjectPreferencesWithSameStepNameButDifferentPackage();
            WhenMergePreferences();
            ThenNamesDontMatchAnymore();
        }

        private void GivenUserPreference()
        {
            userPreference = new UserPreference
            {
                DscResources =
                {
                    new MockDscResource {Ensure = Ensure.Present, ResourceStepName = "Step", ResourceName = "Test"}
                }
            };
        }

        private void ThenUserPreferenceIsApplied()
        {
            Assert.True(mergeResult.Success);
            Assert.Equal(userPreference.DscResources.Count, mergeResult.MergedDscResources.Count);
            foreach (var userPreferenceDscResource in userPreference.DscResources)
            {
                var mergedDscResource = mergeResult.MergedDscResources.Single(
                    x => x.Value.ResourceName.Equals(userPreferenceDscResource.ResourceName));
                Assert.True(mergedDscResource.Success);
                DscResourceTestHelper.AssertDscResourceEqual(userPreferenceDscResource, mergedDscResource.Value);
            }
        }

        private void ThenNamesDontMatchAnymore()
        {
            Assert.NotEqual(1, resultPreference.MergedDscResources.GroupBy(x => x.Value.ResourceStepName).Count());
        }

        private void GivenUserAndProjectPreferencesWithSameStepNameButDifferentPackage()
        {
            dockerChocolateyResource = new MockDscResource
            {
                ResourceStepName = "step1",
                Ensure = Ensure.Absent
            };

            visualStudioChocolateyResource = new MockDscResource2
            {
                Ensure = Ensure.Present,
                ResourceStepName = "step1"
            };

            projectPreference = new ProjectPreference
            {
                DscResources = new List<DscResource> { dockerChocolateyResource }
            };

            userPreference = new UserPreference
            {
                DscResources = new List<DscResource> { visualStudioChocolateyResource }
            };
        }

        private void ThenCorrectResourcesAreCreated()
        {
            Assert.NotNull(resultPreference);
            Assert.Equal(4, resultPreference.MergedDscResources.Count);

            var resultChocoSourceResource = Assert.IsType<MergeResult<DscResource>>(Assert.Single(resultPreference.MergedDscResources, x => x.Value.ResourceStepName.Equals("chocoSource")));
            DscResourceTestHelper.AssertDscResourceEqual(chocolateySourceResource, resultChocoSourceResource.Value);

            var resultDockerAbsentResource = Assert.IsType<MergeResult<DscResource>>(Assert.Single(resultPreference.MergedDscResources, x => x.Value.ResourceStepName.Equals("dockerStep")));
            DscResourceTestHelper.AssertDscResourceEqual(absentDockerChocolateyResource, resultDockerAbsentResource.Value);

            var resultVisualStudioResource = Assert.IsType<MergeResult<DscResource>>(Assert.Single(resultPreference.MergedDscResources,
                x => x.Value.ResourceStepName.Equals("visualStudioStep")));
            DscResourceTestHelper.AssertDscResourceEqual(visualStudioChocolateyResource, resultVisualStudioResource.Value);

            var resultFirefoxSpecifiedResource = Assert.IsType<MergeResult<DscResource>>(Assert.Single(resultPreference.MergedDscResources,
                x => x.Value.ResourceStepName.Equals("firefoxStep")));
            DscResourceTestHelper.AssertDscResourceEqual(firefoxChocolateyResource, resultFirefoxSpecifiedResource.Value);
        }

        private void WhenMergeUserPreferenceWithEmptyPreference()
        {
            mergeResult = userPreference.MergePreference(new EmptyPreferenceMergeResult());
        }

        private void WhenMergePreferences()
        {
            resultPreference =
                projectPreference.MergePreference(userPreference.MergePreference(new EmptyPreferenceMergeResult()));
        }

        private void GivenUserAndProjectPreferences()
        {
            projectPreference = new ProjectPreference
            {
                DscResources = new List<DscResource> { chocolateySourceResource, visualStudioChocolateyResource, absentDockerChocolateyResource }
            };

            userPreference = new UserPreference
            {
                DscResources = new List<DscResource> { firefoxChocolateyResource, dockerChocolateyResource }
            };
        }

        private void InitializeTestDscResources()
        {
            chocolateySourceResource = new MockDscResource
            {
                Ensure = Ensure.Present,
                ResourceStepName = "chocoSource"
            };

            dockerChocolateyResource = new MockDscResource
            {
                ResourceStepName = "dockerStep",
                DependsOn = new List<DscResource> { chocolateySourceResource },
                Ensure = Ensure.Present
            };

            visualStudioChocolateyResource = new MockDscResource2
            {
                Ensure = Ensure.Present,
                ResourceStepName = "visualStudioStep"
            };

            absentDockerChocolateyResource = new MockDscResource
            {
                ResourceStepName = "dockerStep",
                Ensure = Ensure.Absent
            };

            firefoxChocolateyResource = new MockDscResource
            {
                Ensure = Ensure.Present,
                ResourceStepName = "firefoxStep"
            };
        }

        private class MockDscResource2 : DscResource
        {
            public MockDscResource2()
            {
                ResourceName = "Mock2";
            }
            public override DscResourceDto ToResourceDto()
            {
                return new Dto.MergedPreferenceTests.MockDto();
            }
        }
    }
}