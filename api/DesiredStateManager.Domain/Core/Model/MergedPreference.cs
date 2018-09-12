using System;
using System.Collections.Generic;

namespace DesiredStateManager.Domain.Core.Model
{
    public class MergedPreference
    {
        public List<DscResource> DscResources { get; set; }

        public static MergedPreference FromPreferences(ProjectPreference projectPreference,
            UserPreference userPreference)
        {
            throw new NotImplementedException();
        }
    }
}