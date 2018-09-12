using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public class MergeHelper
    {
        public static string GetMergableName(string oldResourceStepName, List<MergeResult<DscResource>> mergeResultsToMerge)
        {
            string newResourceStepName = oldResourceStepName;
            int duplIndex = 1;
            while (mergeResultsToMerge.Count(x => x.IsDuplicateName(newResourceStepName)) > 0)
            {
                newResourceStepName = $"{oldResourceStepName}_{duplIndex}";
            }

            return newResourceStepName;
        }
    }
}