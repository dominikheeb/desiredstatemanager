using System;
using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Core.Dto;

namespace DesiredStateManager.Domain.Core.Model
{
    public class DscConfiguration
    {
        public List<DscResource> DscResources { get; set; }

        public static DscConfiguration FromMergedPreference(MergedPreference projectPreference)
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