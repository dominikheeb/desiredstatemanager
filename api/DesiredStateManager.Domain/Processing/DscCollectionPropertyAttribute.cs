using System;

namespace DesiredStateManager.Domain.Processing
{
    public class DscCollectionPropertyAttribute : Attribute
    {
    public DscCollectionPropertyAttribute(string propertyName, Type collectionType)
    {
        PropertyName = propertyName;
        CollectionType = collectionType;
    }

        public Type CollectionType { get; }

        public string PropertyName { get; }
    }
}