using System.Collections.Generic;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public class ChocolateyPackage : IDscResource
    {
        public Ensure Ensure { get; set; }
        public string ResourceName { get; set; }
        public string ResourceStepName { get; set; }
        public List<IDscResource> DependsOn { get; set; }
    }
}