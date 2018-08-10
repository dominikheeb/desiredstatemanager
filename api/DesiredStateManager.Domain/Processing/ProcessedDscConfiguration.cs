using System.Collections.Generic;
using DesiredStateManager.Domain.Core.Dto;

namespace DesiredStateManager.Domain.Processing
{
    public class ProcessedDscConfiguration
    {
        public List<ProcessedDscResource> ProcessedDscResources { get; set; }

        public static ProcessedDscConfiguration FromDscConfiguration(DscConfigurationDto dscConfiguration)
        {
            return null;
        }
    }
}