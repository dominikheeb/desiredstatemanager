using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Model;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Model;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Core.Model
{
    public class MergedPreferenceTests
    {
        private ChocolateySource chocolateySourceResource;
        private ChocolateyPackage dockerChocolateyResource;
        private ChocolateyPackage visualStudioChocolateyResource;
        private ChocolateyPackage absentDockerChocolateyResource;
        private ChocolateyPackage firefoxChocolateyResource;
        private ChocolateyPackage firefoxSpecifiedVersionResource;
        private ProjectPreference projectPreference;
        private UserPreference userPreference;
        private MergedPreference resultPreference;
        private ChocolateyPackage dockerChocolateyResourceVersion1;
        private ChocolateyPackage dockerChocolateyResourceVersion2;

        [Fact(Skip = "Not implemented")]
        public void TestPreferenceMerge()
        {
            InitializeTestDscResources();
            GivenUserAndProjectPreferences();
            WhenMergePreferences();
            ThenCorrectResourcesAreCreated();
        }

        [Fact(Skip = "Not implemented")]
        public void TestVersionMerge()
        {
            //TODO: Move to chocolatey tests, since it's testing mergin strategy of chocopackages
            GivenUserAndProjectPreferencesWithVersionMissmatch();
            WhenMergePreferences();
            ThenVersionOfProjectWins();
        }

        [Fact(Skip = "Not implemented")]
        public void TestMergeStepNaming()
        {
            GivenUserAndProjectPreferencesWithSameStepNameButDifferentPackage();
            WhenMergePreferences();
            ThenNamesDontMatchAnymore();
        }

        private void ThenNamesDontMatchAnymore()
        {
            Assert.NotEqual(1, resultPreference.DscResources.GroupBy(x => x.ResourceStepName).Count());
        }

        private void GivenUserAndProjectPreferencesWithSameStepNameButDifferentPackage()
        {
            dockerChocolateyResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                ResourceStepName = "step1",
                Ensure = Ensure.Present
            };

            visualStudioChocolateyResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "visualstudio",
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

        private void ThenVersionOfProjectWins()
        {
            Assert.NotNull(resultPreference);
            Assert.NotNull(resultPreference.DscResources);
            var dockerPackage = Assert.IsType<ChocolateyPackage>(Assert.Single(resultPreference.DscResources));
            DscResourceTestHelper.AssertIDscResourceEqual(dockerChocolateyResourceVersion1, dockerPackage);
            Assert.Equal(dockerChocolateyResourceVersion1.ChocolateyPackageName, dockerPackage.ChocolateyPackageName);
            Assert.Equal(dockerChocolateyResourceVersion1.ChocolateyPackageVersion, dockerPackage.ChocolateyPackageVersion);
        }

        private void GivenUserAndProjectPreferencesWithVersionMissmatch()
        {
            dockerChocolateyResourceVersion1 = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                ResourceStepName = "dockerStep",
                Ensure = Ensure.Present,
                ChocolateyPackageVersion = "1.2.0"
            };

            dockerChocolateyResourceVersion2 = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                ResourceStepName = "dockerStep",
                Ensure = Ensure.Present,
                ChocolateyPackageVersion = "2.2.0"
            };

            projectPreference = new ProjectPreference
            {
                DscResources = new List<DscResource> { dockerChocolateyResourceVersion1 }
            };

            userPreference = new UserPreference
            {
                DscResources = new List<DscResource> {dockerChocolateyResourceVersion2}
            };
        }

        private void ThenCorrectResourcesAreCreated()
        {
            Assert.NotNull(resultPreference);
            Assert.Equal(4, resultPreference.DscResources.Count);

            var resultChocoSourceResource = Assert.IsType<ChocolateySource>(Assert.Single(resultPreference.DscResources, x => x.ResourceStepName.Equals("chocoSource")));
            DscResourceTestHelper.AssertIDscResourceEqual(chocolateySourceResource, resultChocoSourceResource);
            Assert.Equal(chocolateySourceResource.ChocoPackageSource, resultChocoSourceResource.ChocoPackageSource);

            var resultDockerAbsentResource = Assert.IsType<ChocolateyPackage>(Assert.Single(resultPreference.DscResources, x => x.ResourceStepName.Equals("dockerStep")));
            DscResourceTestHelper.AssertIDscResourceEqual(absentDockerChocolateyResource, resultDockerAbsentResource);
            Assert.Equal(absentDockerChocolateyResource.ChocolateyPackageName, resultDockerAbsentResource.ChocolateyPackageName);
            Assert.Equal(absentDockerChocolateyResource.ChocolateyPackageVersion, resultDockerAbsentResource.ChocolateyPackageVersion);

            var resultVisualStudioResource = Assert.IsType<ChocolateyPackage>(Assert.Single(resultPreference.DscResources,
                x => x.ResourceStepName.Equals("visualStudioStep")));
            DscResourceTestHelper.AssertIDscResourceEqual(visualStudioChocolateyResource, resultVisualStudioResource);
            Assert.Equal(visualStudioChocolateyResource.ChocolateyPackageName, resultVisualStudioResource.ChocolateyPackageName);
            Assert.Equal(visualStudioChocolateyResource.ChocolateyPackageVersion, resultVisualStudioResource.ChocolateyPackageVersion);

            var resultFirefoxSpecifiedResource = Assert.IsType<ChocolateyPackage>(Assert.Single(resultPreference.DscResources,
                x => x.ResourceStepName.Equals("firefoxStepVersioned")));
            DscResourceTestHelper.AssertIDscResourceEqual(firefoxSpecifiedVersionResource, resultFirefoxSpecifiedResource);
            Assert.Equal(firefoxSpecifiedVersionResource.ChocolateyPackageName, resultFirefoxSpecifiedResource.ChocolateyPackageName);
            Assert.Equal(firefoxSpecifiedVersionResource.ChocolateyPackageVersion, resultFirefoxSpecifiedResource.ChocolateyPackageVersion);
        }

        private void WhenMergePreferences()
        {
            resultPreference = MergedPreference.FromPreferences(projectPreference, userPreference);
        }

        private void GivenUserAndProjectPreferences()
        {
            projectPreference = new ProjectPreference
            {
                DscResources = new List<DscResource> { chocolateySourceResource, visualStudioChocolateyResource, absentDockerChocolateyResource, firefoxSpecifiedVersionResource }
            };

            userPreference = new UserPreference
            {
                DscResources = new List<DscResource> { firefoxChocolateyResource, dockerChocolateyResource }
            };
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
                DependsOn = new List<DscResource> { chocolateySourceResource },
                Ensure = Ensure.Present
            };

            visualStudioChocolateyResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "visualstudio",
                Ensure = Ensure.Present,
                ResourceStepName = "visualStudioStep"
            };

            absentDockerChocolateyResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "docker-for-windows",
                ResourceStepName = "dockerStep",
                Ensure = Ensure.Absent
            };

            firefoxChocolateyResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "firefox",
                Ensure = Ensure.Present,
                ResourceStepName = "firefoxStep"
            };

            firefoxSpecifiedVersionResource = new ChocolateyPackage
            {
                ChocolateyPackageName = "firefox",
                Ensure = Ensure.Present,
                ResourceStepName = "firefoxStepVersioned",
                ChocolateyPackageVersion = "1.23"
            };
        }
    }
}