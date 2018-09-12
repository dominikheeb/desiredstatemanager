using System;
using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public class ChocolateyPackage : IDscResource
    {
        public ChocolateyPackage()
        {
            ResourceName = "cChocoPackageInstaller";
        }

        public Ensure Ensure { get; set; }
        public string ResourceName { get; set; }
        public string ResourceStepName { get; set; }
        public List<IDscResource> DependsOn { get; set; }
        public DscResourceDto ToResourceDto()
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

        public List<MergeResult<IDscResource>> MergeDscResources(List<MergeResult<IDscResource>> mergeResultsToMerge)
        {
            mergeResultsToMerge = mergeResultsToMerge.Where(x => !IsOverriddenByThis(x)).ToList();
            ResourceStepName = MergeHelper.GetMergableName(ResourceStepName, mergeResultsToMerge);

            mergeResultsToMerge.Add(new MergeResult<IDscResource>
            {
                Value = this,
                Success = true
            });

            return mergeResultsToMerge;
        }

        private bool IsOverriddenByThis(MergeResult<IDscResource> mergeResult)
        {
            bool chocolateySourceIsOverriddenByThis = false;
            if (mergeResult.Value is ChocolateyPackage chocolateySourceToCompare)
            {
                chocolateySourceIsOverriddenByThis = chocolateySourceToCompare.ChocolateyPackageName.Equals(ChocolateyPackageName);
            }

            return mergeResult.Value.IsOverriddenByNaming(this) || 
                   chocolateySourceIsOverriddenByThis;
        }

        public string ChocolateyPackageName { get; set; }

        public string ChocolateyPackageVersion { get; set; }
    }
}