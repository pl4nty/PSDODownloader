# PSDODownloader

PowerShell client for Delivery Optimization on Windows, based on [DODownloaderDotNet](https://github.com/shishirb-MSFT/DODownloaderDotNet).

## Features

- Download files using Delivery Optimization with familiar [Invoke-WebRequest](https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/invoke-webrequest) syntax
- List existing downloads, including properties that aren't available with `Get-DeliveryOptimizationStatus`

## Setup

```powershell
Install-Module PSDODownloader
```

Prerelease builds can be installed in PowerShell 7 from the [GitHub Packages repository](https://github.com/pl4nty/PSDODownloader/pkgs/nuget/PSDODownloader), using a [classic Personal Access Token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens#creating-a-personal-access-token-classic) with the `read:packages` scope:

```powershell
Register-PSResourceRepository -Name pl4nty -Uri https://nuget.pkg.github.com/pl4nty/index.json
Install-PSResource -Name PSDODownloader -Repository pl4nty -Credential (Get-Credential) -Prerelease
```

## Usage

See [Invoke-DORequest](./docs/Invoke-DORequest.md) and [Get-DORequests](./docs/Get-DORequests.md).
