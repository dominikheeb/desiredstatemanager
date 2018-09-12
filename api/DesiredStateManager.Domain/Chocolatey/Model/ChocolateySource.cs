using System;
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
        public ChocolateySource()
        {
            ResourceName = "cChocoSource";
        }

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

        private bool IsOverriddenByThis(MergeResult<IDscResource> resultToCompare)
        {
            bool chocolateySourceIsOverriddenByThis = false;
            if(resultToCompare.Value is ChocolateySource chocolateySourceToCompare)
            {
                chocolateySourceIsOverriddenByThis = chocolateySourceToCompare.ChocoPackageSource.Equals(ChocoPackageSource);
            }

            return resultToCompare.Value.IsOverriddenByNaming(this) ||
                   chocolateySourceIsOverriddenByThis;
        }

        public string ChocoPackageSource { get; set; }
    }
}