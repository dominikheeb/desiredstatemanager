using System;
using System.Collections.Generic;
using System.Linq;
using DesiredStateManager.Domain.Processing;
using Xunit;

namespace DesiredStateManager.Domain.Tests
{
    public class DscCollectionPropertyAttributeTests
    {
        public static Type GetNullableUnderlying(Type nullableType)
        {
            return Nullable.GetUnderlyingType(nullableType) ?? nullableType;
        }

        [Fact]
        public void TestCollectionPropertyOnlyUsedInCollections()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());
            var allPropertyInfos = allTypes.SelectMany(a => a.GetProperties()).ToArray();

            foreach (var propertyInfo in allPropertyInfos)
            {
                var propertyType = GetNullableUnderlying(propertyInfo.PropertyType);
                if (propertyInfo.GetCustomAttributes(true).OfType<DscCollectionPropertyAttribute>().Any())
                    Assert.True(typeof(IEnumerable<object>).IsAssignableFrom(propertyType),
                        $"Property {propertyInfo.Name} of Type {propertyInfo.PropertyType} in {propertyInfo.DeclaringType} found using DscCollectionProperty not being a IEnumerable");
            }
        }
    }
}