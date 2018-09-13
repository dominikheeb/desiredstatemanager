using System.Linq;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Tests.Core.Model
{
    public partial class MergedPreferenceTests
    {
        public class MockDscResource : DscResource
        {
            public MockDscResource()
            {
                ResourceName = "Mock";
            }

            public override DscResourceDto ToResourceDto()
            {
                return new Dto.MergedPreferenceTests.MockDto
                {
                    ResourceStepName = ResourceStepName,
                    DependsOn = DependsOn.Select(x => x.ToResourceDto()).ToList(),
                    Ensure = Ensure,
                    ResourceName = ResourceName
                };
            }
        }
    }
}