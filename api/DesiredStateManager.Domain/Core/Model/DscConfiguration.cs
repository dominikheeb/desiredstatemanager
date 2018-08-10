using System.Collections.Generic;

namespace DesiredStateManager.Domain.Core.Model
{
    public class DscConfiguration
    {
        public List<IDscResource> DscResources { get; set; }

        public static DscConfiguration FromPreferences(ProjectPreference projectPreference,
            UserPreference userPreference)
        {
            return null;
        }
    }
}