﻿using System.Collections.Generic;
using DesiredStateManager.Domain.Core;
using DesiredStateManager.Domain.Core.Dto;
using DesiredStateManager.Domain.Core.Model;

namespace DesiredStateManager.Domain.Chocolatey.Model
{
    public class ChocolateyPackage : IDscResource
    {
        public ChocolateyPackage()
        {
            ResourceName = "cChocoPackageInstaller";
        }

        public Ensure Ensure { get; set; }
        public string ResourceName { get; set; }
        public string ResourceStepName { get; set; }
        public List<IDscResource> DependsOn { get; set; }
        public DscResourceDto ToResourceDto()
        {
            throw new System.NotImplementedException();
        }

        public string ChocolateyPackageName { get; set; }
    }
}