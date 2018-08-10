using System;

namespace DesiredStateManager.Domain.Processing
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DscPropertyAttribute : Attribute
    {
        public DscPropertyAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        public string PropertyName { get; }
    }
}