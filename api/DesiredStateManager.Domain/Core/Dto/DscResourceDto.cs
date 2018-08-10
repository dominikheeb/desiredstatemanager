using System;
using System.Collections.Generic;
using DesiredStateManager.Domain.Processing;

namespace DesiredStateManager.Domain.Core.Dto
{
    public abstract class DscResourceDto
    {
        [DscProperty("Ensure")]
        public Ensure Ensure { get; set; }

        public string ResourceName { get; set; }

        public string ResourceStepName { get; set; }

        [DscCollectionProperty("DependsOn", typeof(DscResourceDto))]
        public List<DscResourceDto> DependsOn { get; set; }

        public override string ToString()
        {
            return $"[{ResourceName}]{ResourceStepName}";
        }
    }
}