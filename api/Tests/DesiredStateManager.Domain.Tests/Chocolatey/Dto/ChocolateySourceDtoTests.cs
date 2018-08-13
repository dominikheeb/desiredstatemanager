using DesiredStateManager.Domain.Chocolatey.Dto;
using DesiredStateManager.Domain.Core;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Chocolatey.Dto
{
    public class ChocolateySourceDtoTests
    {
        private ChocolateySourceDto chocolateySourceDto;
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
            Assert.Equal($"[{chocolateySourceDto.ResourceName}]{chocolateySourceDto.ResourceStepName}", resultString);
        }

        private void WhenRunningToString()
        {
            resultString = chocolateySourceDto.ToString();
        }

        private void GivenDtoWithValues()
        {
            chocolateySourceDto = new ChocolateySourceDto
            {
                ResourceName = "cChocoPackageInstaller",
                ChocoPackageSource = "https://Test123s",
                ResourceStepName = "dockerStep",
                Ensure = Ensure.Present
            };
        }
    }
}