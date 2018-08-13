using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public class ChocolateySource : IDscResource
    {
        public Ensure Ensure { get; set; }
        public string ResourceName { get; set; }
        public string ResourceStepName { get; set; }
        public List<IDscResource> DependsOn { get; set; }
        public DscResourceDto ToResourceDto()
        {
            return new ChocolateySourceDto
            {
                ChocoPackageSource = ChocoPackageSource,
                Ensure = Ensure,
                ResourceStepName = ResourceStepName,
                ResourceName = ResourceName,
                DependsOn = DependsOn?.Select(x => x.ToResourceDto()).ToList()
            };
        }

        public string ChocoPackageSource { get; set; }
    }
}