using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Chocolatey.Model;

namespace DesiredStateManager.Domain.Core.Model
{
    public abstract class Preference
    {
        public List<DscResource> DscResources { get; set; } = new List<DscResource>();

        public MergedPreference MergePreference(MergedPreference preferenceToMerge)
        {
            var mergedDscResources = preferenceToMerge.MergedDscResources;

            foreach (var dscResource in DscResources)
            {
                mergedDscResources = dscResource.MergeDscResources(mergedDscResources);
            }

            return new MergedPreference(mergedDscResources);
        }
    }
}