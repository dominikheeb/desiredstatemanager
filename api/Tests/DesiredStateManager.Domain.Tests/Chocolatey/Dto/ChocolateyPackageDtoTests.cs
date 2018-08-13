using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Chocolatey.Dto
{
    public class ChocolateyPackageDtoTests
    {
        private ChocolateyPackageDto dockerChocolateyResourceDto;
        private string resultString;

        [Fact]
        public void TestToString()
        {
            GivenDtoWithValues();
            WhenRunningToString();
            ThenDscFormatIsCreated();
        }

        private void ThenDscFormatIsCreated()
        {
            Assert.Equal($"[{dockerChocolateyResourceDto.ResourceName}]{dockerChocolateyResourceDto.ResourceStepName}", resultString);
        }

        private void WhenRunningToString()
        {
            resultString = dockerChocolateyResourceDto.ToString();
        }
    
        private void GivenDtoWithValues()
        {
            dockerChocolateyResourceDto = new ChocolateyPackageDto
            {
                ResourceName = "cChocoPackageInstaller",
                ChocolateyPackageName = "docker-for-windows",
                ResourceStepName = "dockerStep",
                Ensure = Ensure.Present
            };
        }
    }
}