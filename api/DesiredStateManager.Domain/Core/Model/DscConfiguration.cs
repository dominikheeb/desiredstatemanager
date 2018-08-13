using System;
using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Core.Dto;

namespace DesiredStateManager.Domain.Core.Model
{
    public class DscConfiguration
    {
        public List<IDscResource> DscResources { get; set; }

        public static DscConfiguration FromPreferences(ProjectPreference projectPreference,
            UserPreference userPreference)
        {
            throw new NotImplementedException();
        }

        public DscConfigurationDto ToConfigurationDto()
        {
            return new DscConfigurationDto
            {
                DscResourceDtos = DscResources?.Select(x => x.ToResourceDto()).ToList()
            };
        }
    }
}