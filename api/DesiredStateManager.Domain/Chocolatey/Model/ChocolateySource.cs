using System;
using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public class ChocolateySource : DscResource
    {
        public ChocolateySource()
        {
            ResourceName = "cChocoSource";
        }
        public string ChocoPackageSource { get; set; }

        public override DscResourceDto ToResourceDto()
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

        internal override bool IsOverriddenByThis(MergeResult<DscResource> resultToCompare)
        {
            bool chocolateySourceIsOverriddenByThis = false;
            if(resultToCompare.Value is ChocolateySource chocolateySourceToCompare)
            {
                chocolateySourceIsOverriddenByThis = chocolateySourceToCompare.ChocoPackageSource.Equals(ChocoPackageSource);
            }

            return base.IsOverriddenByThis(resultToCompare)||
                   chocolateySourceIsOverriddenByThis;
        }
    }
}