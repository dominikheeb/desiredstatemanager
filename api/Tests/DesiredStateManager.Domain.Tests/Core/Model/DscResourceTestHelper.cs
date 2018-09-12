using DesiredStateManager.Domain.Core.Model;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Core.Model
{
    public static class DscResourceTestHelper
    {
        public static void AssertIDscResourceEqual(DscResource expectedDscResource, DscResource actualDscResource)
        {
            Assert.Equal(expectedDscResource.ResourceStepName, actualDscResource.ResourceStepName);
            Assert.Equal(expectedDscResource.ResourceName, actualDscResource.ResourceName);
            Assert.Equal(expectedDscResource.Ensure, actualDscResource.Ensure);
            Assert.Equal(expectedDscResource.DependsOn, expectedDscResource.DependsOn);
        }
    }
}