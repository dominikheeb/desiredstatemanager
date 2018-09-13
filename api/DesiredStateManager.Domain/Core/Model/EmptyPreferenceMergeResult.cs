using System.Collections.Generic;

namespace DesiredStateManager.Domain.Core.Model
{
    public class EmptyPreferenceMergeResult : MergedPreference
    {
        public EmptyPreferenceMergeResult() : base(new List<MergeResult<DscResource>>())
        {
        }
    }
}