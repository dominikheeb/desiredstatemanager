param (    
    [Parameter(Mandatory = $true)]
    [String] $Path,

    [String] $DependenciesPath = (Join-Path $Path "Libs")
) 

$DependencyFiles = Get-ChildItem -Path (Join-Path $Path "*.depend.psd1") -Recurse

Write-Host "Dependency Files found: $DependencyFiles"

$dependencyFilesContents = ($DependencyFiles | Foreach-Object { 
    $dependencyContent = Import-PowerShellDataFile $_.FullName 
    Write-Host "Found $($dependencyContent.Count) dependencies for $($_.Name)" 
    $dependencyContent 
} | Where-Object -Property Count -gt 0)

$dependenciesPerLibrary = $dependencyFilesContents |
    Foreach-Object { $_.GetEnumerator() } |
    Select-Object -Property Key, Value -Unique | 
    Group-Object -Property Key

$duplicates = $dependenciesPerLibrary |
    Where-Object -Property Count -gt 1

if($duplicates.Count -gt 0){
    $duplicates | ForEach-Object {
        $conflictingVersions = ($_.Group | Select-Object -Property Value -ExpandProperty Value)
        Write-Warning "Multiple dependencies on one library with different versions -> $($_.Name) Versions: $conflictingVersions"
    }
    throw "Multiple dependencies on one library with different versions"
}

If(test-path $DependenciesPath)
{
    Remove-Item $DependenciesPath -Force -Recurse
}

$resolvedDependencies = @{}

New-Item -ItemType Directory -Path $DependenciesPath
if($dependenciesPerLibrary -ne $null){
    $dependenciesPerLibrary.Group | Foreach-Object {
        Write-Host "Downloading Module $($_.Key) with Version '$($_.Value)'"
        if($_.Value -eq "latest"){
            Save-Module -Name $_.Key -Repository 'PSGallery' -Path $DependenciesPath -Force -Verbose
        }
        else{
            $latestVersion = (Find-Module -Name $_.Key -Repository 'PSGallery' -MinimumVersion $_.Value).Version.ToString()
            if($latestVersion -ne $_.Value){
                Write-Host "Update available for dependency $($_.Key). Current: $($_.Value), latest: $latestVersion"
            }

            Save-Module -Name $_.Key -RequiredVersion $_.Value -Repository 'PSGallery' -Path $DependenciesPath -Force -Verbose
        }    

        $resolvedDependencies.Add($_.Key, $_.Value)
    }
}

$output = "@{`r`n"
$resolvedDependencies.Keys | ForEach-Object { 
    $output += "$($_)='$($resolvedDependencies[$_])'`r`n" 
}
$output += "}"
$output | Out-File (Join-Path $DependenciesPath "resolvedDependencies.psd1")

