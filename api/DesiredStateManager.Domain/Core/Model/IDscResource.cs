using System.Collections.Generic;
using DesiredStateManager.Domain.Core.Dto;

namespace DesiredStateManager.Domain.Core.Model
{
    public interface IDscResource
    {
        Ensure Ensure { get; set; }

        string ResourceName { get; set; }

        string ResourceStepName { get; set; }

        List<IDscResource> DependsOn { get; set; }

        DscResourceDto ToResourceDto();
    }
}