using System.Collections.Generic;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Processing.Services;

namespace DesiredStateManager.Domain.Processing.Model
{
    public class ProcessedDscConfiguration
    {
        public ProcessedDscConfiguration()
        {
            ProcessedDscResources = new List<ProcessedDscResource>();
        }
        public List<ProcessedDscResource> ProcessedDscResources { get; set; }

        public static ProcessedDscConfiguration FromDscConfiguration(DscConfigurationDto dscConfiguration)
        {
            var result = new ProcessedDscConfiguration
            {
                ProcessedDscResources =
                DscResourceProcessingHelper.SortAndProcessDscResources(dscConfiguration.DscResourceDtos)
            };

            return result;
        }
    }
}