param (
    [String] $OutputFolder = "$psscriptroot\..\MOF\DevMachine",

    [String] $DependenciesPath = "$psscriptroot\Libs",

    [String] $ModuleInstallationDir = "$Env:ProgramFiles\WindowsPowerShell\Modules"
)

$requiredModules = Import-PowerShellDataFile "$psscriptroot\devMachine.depend.psd1"

foreach ($moduleName in $requiredModules.Keys)
{
    $modulePath = Join-Path $DependenciesPath $moduleName
    If(!(Test-Path $modulePath)){
        Write-Host "Module $moduleName not found!"
        exit 1
    }
    
    Write-Host "Importing module $moduleName"
    Copy-Item -Path $modulePath -Destination $ModuleInstallationDir -Force -Recurse
}

$cd = @{
    AllNodes = @(
        @{
            NodeName = 'localhost'
            PSDscAllowDomainUser = $true
            PSDscAllowPlainTextPassword = $true
        }
    )
}

. "$psscriptroot\devMachineConfiguration.ps1"
DevMachine -OutputPath $OutputFolder -ConfigurationData $cd