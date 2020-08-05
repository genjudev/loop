$ErrorActionPreference = 'Stop'

$toolsDir   = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
$defaultDotnetRuntimePath = "C:\Program Files\dotnet\dotnet.exe"

if (!(Test-Path $defaultDotnetRuntimePath))
{
    Write-Host -ForegroundColor Red "File not found: $defaultDotnetRuntimePath"
    Write-Host "The package depends on the .NET Core Runtime (dotnet.exe) which was not found."
    Write-Host "Please install the latest version of the .NET Core Runtime to use this package."
    exit 1
}

Install-Binfile -Name loop -Path "$defaultDotnetRuntimePath" -Command "$toolsDir\loop.exe"