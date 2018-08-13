using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Processing;

namespace DesiredStateManager.Domain.Chocolatey.Dto
{
    public class ChocolateyPackageDto : DscResourceDto
    {
        [DscProperty("Name")]
        public string ChocolateyPackageName { get; set; }

        [DscProperty("Version")]
        public string ChocolateyPackageVersion { get; set; }
    }
}