using System.Collections.Generic;

namespace DesiredStateManager.Domain.Core.Model
{
    public interface IDscResource
    {
        Ensure Ensure { get; set; }

        string ResourceName { get; set; }

        string ResourceStepName { get; set; }

        List<IDscResource> DependsOn { get; set; }
    }
}