using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Processing.Model;

namespace DesiredStateManager.Domain.Processing.Services
{
    public static class DscResourceProcessingHelper
    {
        public static List<ProcessedDscResource> SortAndProcessDscResources(List<DscResourceDto> resourceDtos)
        {
            var sortedDscResourceDtos = TopologicalSortDscResourceDtos(resourceDtos);
            return ProcessDscResourceDtos(sortedDscResourceDtos);
        }

        private static List<ProcessedDscResource> ProcessDscResourceDtos(List<DscResourceDto> dscResourceDtos)
        {
            var processedDscResources = new List<ProcessedDscResource>();

            foreach (var dscResourceDto in dscResourceDtos)
            {
                var processedDscResource = new ProcessedDscResource
                {
                    ResourceName = dscResourceDto.ResourceName,
                    ResourceStepName = dscResourceDto.ResourceStepName
                };


                var propertyInfos = dscResourceDto.GetType().GetProperties();
                var dscProperties = propertyInfos.Where(x => x.GetCustomAttribute<DscPropertyAttribute>() != null).Select(x =>
                    new
                    {
                        x.GetCustomAttribute<DscPropertyAttribute>().PropertyName,
                        PropertyValue = x.GetValue(dscResourceDto)
                    }).Where(x => x.PropertyValue != null);

                var dscCollectionProperties = propertyInfos.Where(x => x.GetCustomAttribute<DscCollectionPropertyAttribute>() != null).Select(x =>
                    new
                    {
                        x.GetCustomAttribute<DscCollectionPropertyAttribute>().PropertyName,
                        x.GetCustomAttribute<DscCollectionPropertyAttribute>().CollectionType,
                        PropertyValue = x.GetValue(dscResourceDto)
                    }).Where(x => x.PropertyValue != null);

                foreach (var dscProperty in dscProperties)
                {
                    processedDscResource.DscProperties.Add(dscProperty.PropertyName, dscProperty.PropertyValue.ToString());
                }

                foreach (var dscCollectionProperty in dscCollectionProperties)
                {
                    if (dscCollectionProperty.PropertyValue is IEnumerable<object> collectionValue)
                    {
                        var list = collectionValue.ToList();
                        if (list.Count > 1)
                        {
                            processedDscResource.DscProperties.Add(dscCollectionProperty.PropertyName, $"@({string.Join(", ", list)})");
                        }
                        else if(list.Count == 1)
                        {
                            processedDscResource.DscProperties.Add(dscCollectionProperty.PropertyName, list[0].ToString());
                        }
                    }
                    else
                    {
                        throw new ArgumentException($"Property {dscCollectionProperty.PropertyName} is not a collection");
                    }
                }

                processedDscResources.Add(processedDscResource);
            }

            return processedDscResources;
        }

        private static List<DscResourceDto> TopologicalSortDscResourceDtos(IEnumerable<DscResourceDto> source)
        {
            var sorted = new List<DscResourceDto>();
            var visited = new Dictionary<string, bool>();

            foreach (var item in source)
            {
                Visit(item, sorted, visited);
            }

            return sorted;
        }

        private static void Visit(DscResourceDto item, ICollection<DscResourceDto> sorted, Dictionary<string, bool> visited)
        {
            var alreadyVisited = visited.TryGetValue(item.CreateIdForTopologicalSort(), out var inProcess);

            if (alreadyVisited)
            {
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                visited[item.CreateIdForTopologicalSort()] = true;

                var dependencies = item.DependsOn;
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        Visit(dependency, sorted, visited);
                    }
                }

                visited[item.CreateIdForTopologicalSort()] = false;
                sorted.Add(item);
            }
        }


        private static string CreateIdForTopologicalSort(this DscResourceDto dscResourceDto)
        {
            return $"[{dscResourceDto.ResourceName}]{dscResourceDto.ResourceStepName}";
        }
    }
}