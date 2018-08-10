using System.Collections.Generic;

namespace DesiredStateManager.Domain.Core.Dto
{
    public class DscConfigurationDto
    {
        public DscConfigurationDto()
        {
            DscResourceDtos = new List<DscResourceDto>();
        }

        public List<DscResourceDto> DscResourceDtos { get; set; }
    }
}