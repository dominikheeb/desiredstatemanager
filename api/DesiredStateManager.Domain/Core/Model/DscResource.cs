using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Model;
using DesiredStateManager.Domain.Core.Dto;

namespace DesiredStateManager.Domain.Core.Model
{
    public abstract class DscResource
    {
        public abstract DscResourceDto ToResourceDto();

        internal virtual bool IsOverriddenByThis(MergeResult<DscResource> mergeResult)
        {
            return mergeResult.Value.IsOverriddenByNaming(this);
        }

        public Ensure Ensure { get; set; }

        public string ResourceName { get; set; }

        public string ResourceStepName { get; set; }

        public List<DscResource> DependsOn { get; set; }

        public List<MergeResult<DscResource>> MergeDscResources(List<MergeResult<DscResource>> mergeResultsToMerge)
        {
            mergeResultsToMerge = mergeResultsToMerge.Where(x => !IsOverriddenByThis(x)).ToList();
            ResourceStepName = MergeHelper.GetMergableName(ResourceStepName, mergeResultsToMerge);

            mergeResultsToMerge.Add(new MergeResult<DscResource>
            {
                Value = this,
                Success = true
            });

            return mergeResultsToMerge;
        }
    }
}