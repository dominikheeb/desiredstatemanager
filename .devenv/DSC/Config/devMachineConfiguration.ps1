Configuration DevMachine
{
    param (
    )

    Import-DscResource -ModuleName 'PSDesiredStateConfiguration'
    Import-DscResource -Module cChoco
    Import-DscResource -Module xComputerManagement

    
    Node localhost
    {   
        xPowerPlan SetPlanHighPerformance
        {
            IsSingleInstance = 'Yes'
            Name             = 'High performance'
        }

        cChocoInstaller installChoco
        {
            InstallDir = "C:\ProgramData\chocolatey"
            DependsOn = "[xPowerPlan]SetPlanHighPerformance"
            PsDscRunAsCredential = $ServiceCredential
        }

        cChocoPackageInstaller installGit
        {
           Ensure = 'Present'
           Name = "git"
           DependsOn = "[cChocoInstaller]installChoco"
        }

        cChocoPackageInstaller installVSCode
        {
           Ensure = 'Present'
           Name = "visualstudiocode"
           DependsOn = "[cChocoInstaller]installChoco"
        }

        cChocoPackageInstaller installDocker
        {
           Ensure = 'Present'
           Name = "docker-for-windows"
           DependsOn = "[cChocoInstaller]installChoco"
        }

        cChocoPackageInstaller installSourceTree
        {
           Ensure = 'Present'
           Name = "sourcetree"
           DependsOn = "[cChocoPackageInstaller]installGit"
        }

        cChocoPackageInstaller installVisualStudio
        {
           Ensure = 'Present'
           Name = "visualstudio2017enterprise"
           DependsOn = "[cChocoInstaller]installChoco"
        }

        cChocoPackageInstaller installVisualStudioWebWorkflow
        {
           Ensure = 'Present'
           Name = "visualstudio2017-workload-netweb"
           DependsOn = "[cChocoPackageInstaller]installVisualStudio"
        }

        Script AddUsersToDockerGroup {
            GetScript = { 
                @{ Result = Get-LocalGroupMember -Group 'docker-users' } 
            }

            SetScript = { 
                Add-LocalGroupMember -Group 'docker-users' -Member 'Users'
            }

            TestScript = { 
                [bool] ((Get-LocalGroupMember -Group 'docker-users').Name -contains ('BuiltIn\Users'))
            }
            DependsOn = "[cChocoPackageInstaller]installDocker"
        }
    }
}