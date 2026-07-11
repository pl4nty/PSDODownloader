# PSDODownloader

PowerShell client for Delivery Optimization (DO) on Windows, based on [DODownloaderDotNet](https://github.com/shishirb-MSFT/DODownloaderDotNet).
Useful for testing DO configuration, cache servers, and performance. Also check out [my tool](https://msft-store.tplant.com.au/) to generate DO URLs, and Microsoft's [official DeliveryOptimization cmdlets](https://learn.microsoft.com/en-us/powershell/module/deliveryoptimization/).

## Features

- Download files using Delivery Optimization with familiar [Invoke-WebRequest](https://learn.microsoft.com/powershell/module/microsoft.powershell.utility/invoke-webrequest) syntax
- List existing downloads, including properties that aren't available with `Get-DeliveryOptimizationStatus`

## Setup

```powershell
Install-Module PSDODownloader
```

Prerelease builds can be installed from [GitHub Packages](https://github.com/pl4nty/PSDODownloader/pkgs/nuget/PSDODownloader) in PowerShell 7, using a public proxy:

```powershell
Register-PSResourceRepository -Name pl4nty -Uri https://github-nuget.tplant.com.au/pl4nty/index.json
Install-PSResource -Name PSDODownloader -Repository pl4nty -Prerelease
```

## Usage

See [Invoke-DORequest](./docs/Invoke-DORequest.md) and [Get-DORequests](./docs/Get-DORequests.md).
