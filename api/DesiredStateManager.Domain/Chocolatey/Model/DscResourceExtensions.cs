using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public static class DscResourceExtensions
    {
        public static bool IsOverriddenByNaming(this IDscResource resourceToBeOverridden, IDscResource resourceToCompare)
        {
            return resourceToBeOverridden.ResourceName.Equals(resourceToCompare.ResourceName) &&
                   resourceToBeOverridden.ResourceStepName.Equals(resourceToCompare.ResourceStepName);
        }

        public static bool IsDuplicateName(this MergeResult<IDscResource> resultToCompare, string resourceStepName)
        {
            return resultToCompare.Value.ResourceStepName.Equals(resourceStepName) &&
                !resultToCompare.Value.GetType().IsAssignableFrom(typeof(ChocolateySource));
        }
    }
}