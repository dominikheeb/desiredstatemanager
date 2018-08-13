using System;
using System.Collections.Generic;
using DesiredStateManager.Domain.Core.Dto;

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

        public DscConfigurationDto ToConfigurationDto()
        {
            throw new NotImplementedException();
        }
    }
}