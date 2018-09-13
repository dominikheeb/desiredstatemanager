using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DesiredStateManager.Domain.Core.Model
{
    public class MergedPreference
    {
        public List<MergeResult<DscResource>> MergedDscResources { get; set; }
        public bool Success { get; set; }

        public MergedPreference(List<MergeResult<DscResource>> mergedDscResources)
        {
            MergedDscResources = mergedDscResources;
            Success = mergedDscResources.All(x => x.Success);
        }
    }
}