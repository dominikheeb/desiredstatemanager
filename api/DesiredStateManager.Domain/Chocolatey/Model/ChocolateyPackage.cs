using System;
using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public class ChocolateyPackage : DscResource
    {
        public ChocolateyPackage()
        {
            ResourceName = "cChocoPackageInstaller";
        }

        public override DscResourceDto ToResourceDto()
        {
            return new ChocolateyPackageDto
            {
                ChocolateyPackageName = ChocolateyPackageName,
                ResourceStepName = ResourceStepName,
                Ensure = Ensure,
                ResourceName = ResourceName,
                ChocolateyPackageVersion = ChocolateyPackageVersion,
                DependsOn = DependsOn?.Select(x => x.ToResourceDto()).ToList()
            };
        }

        internal override bool IsOverriddenByThis(MergeResult<DscResource> mergeResult)
        {
            bool chocolateySourceIsOverriddenByThis = false;
            if (mergeResult.Value is ChocolateyPackage chocolateySourceToCompare)
            {
                chocolateySourceIsOverriddenByThis = chocolateySourceToCompare.ChocolateyPackageName.Equals(ChocolateyPackageName);
            }

            return base.IsOverriddenByThis(mergeResult) || 
                   chocolateySourceIsOverriddenByThis;
        }

        public string ChocolateyPackageName { get; set; }

        public string ChocolateyPackageVersion { get; set; }
    }
}