using System.Collections.Generic;

namespace DesiredStateManager.Domain.Processing
{
    public class ProcessedDscResource
    {
        public string ResourceName { get; set; }

        public string ResourceStepName { get; set; }

        public Dictionary<string, string> DscProperties { get; set; }
    }
}