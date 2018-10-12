using System;
using System.Collections.Generic;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Processing;
using DesiredStateManager.Domain.Processing.Services;
using Xunit;

namespace DesiredStateManager.Domain.Tests.Processing
{
    public class DscResourceProcessingHelperTests
    {
        private AttributeIssueTestClass attributeIssueTestClass;

        [Fact]
        public void TestErrorWhenCollectionPropertyOnNonCollection()
        {
            GivenDscResourceDtoWithWrongAttribute();
            
            ThenExceptionIsThrown(WhenUsingDscResourceProcessingHelper);
        }

        private void ThenExceptionIsThrown(Action whenMethod)
        {
            Assert.Throws<ArgumentException>(whenMethod);
        }

        private void WhenUsingDscResourceProcessingHelper()
        {
            DscResourceProcessingHelper.SortAndProcessDscResources(new List<DscResourceDto> {attributeIssueTestClass});
        }

        private void GivenDscResourceDtoWithWrongAttribute()
        {
            attributeIssueTestClass = new AttributeIssueTestClass();
        }

        internal class AttributeIssueTestClass : DscResourceDto
        {
            [DscCollectionProperty("TEst", typeof(int))]
            public int Test { get; set; }
        }
    }
}