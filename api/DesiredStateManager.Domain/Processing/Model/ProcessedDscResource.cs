using System.Collections.Generic;

namespace DesiredStateManager.Domain.Processing.Model
{
    public class ProcessedDscResource
    {
        public ProcessedDscResource()
        {
            DscProperties = new Dictionary<string, string>();    
        }

        public string ResourceName { get; set; }

        public string ResourceStepName { get; set; }

        public Dictionary<string, string> DscProperties { get; set; }
    }
}