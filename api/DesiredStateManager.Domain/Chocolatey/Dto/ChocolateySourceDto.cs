using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Processing;

namespace DesiredStateManager.Domain.Chocolatey.Dto
{
    public class ChocolateySourceDto : DscResourceDto
    {
        [DscProperty("Source")]
        public string ChocoPackageSource { get; set; }
    }
}