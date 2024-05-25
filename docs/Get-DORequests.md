---
external help file: DODownloader.dll-Help.xml
Module Name: PSDODownloader
online version:
schema: 2.0.0
---

# Get-DORequests

## SYNOPSIS

Get Delivery Optimization downloads.

## SYNTAX

```powershell
Get-DORequests [-Uri <Uri>] [<CommonParameters>]
```

## DESCRIPTION

The `Get-DORequests` cmdlet gets Delivery Optimization downloads present on the device, including cached downloads.
It returns downloads and their properties from [DODownloadProperty](https://learn.microsoft.com/en-us/windows/win32/api/deliveryoptimization/ne-deliveryoptimization-dodownloadproperty). Some properties may be null and produce a verbose log.

## EXAMPLES

### Example 1

Get all downloads.

```powershell
PS C:\> Get-DORequests

Id                                 : 4a5ada83-a9a6-4cca-9404-a58414d269a2
Uri                                : http://dl.delivery.mp.microsoft.com/filestreamingservice/files/52fa8751-747d-479d-8f22-e32730cc0eb1
ContentId                          : 
DisplayName                        : WU Client Download
LocalPath                          : 
HttpCustomHeaders                  : 
CostPolicy                         : 0
SecurityFlags                      : 0
CallbackFreqPercent                : 
CallbackFreqSeconds                : 1
NoProgressTimeoutSeconds           : 1800
ForegroundPriority                 : True
BlockingMode                       : False
SecurityContext                    : 
NetworkToken                       : 
CorrelationVector                  : qgTBxRZBOk2hE4E7.10
DecryptionInfo                     : 
IntegrityCheckInfo                 : 
IntegrityCheckMandatory            : False
TotalSizeBytes                     : 25006511
DisallowOnCellular                 : False
HttpCustomAuthHeaders              : 
HttpAllowSecureToNonSecureRedirect : 
NonVolatile                        : False
```

### Example 2

Get downloads with a specific URI.

```powershell
PS C:\> Get-DORequests -Uri http://b.c2r.ts.cdn.office.net/pr/492350f6-3a01-4f97-b9c0-c7c6ddf67d60/Office/Data/16.0.17531.20152/i640.c2rx

Id                                 : 50527b41-ca8e-45c5-9aa2-95fd52d20edb
Uri                                : http://b.c2r.ts.cdn.office.net/pr/492350f6-3a01-4f97-b9c0-c7c6ddf67d60/Office/Data/16.0.17531.20152/i640.c2rx
ContentId                          : 
DisplayName                        : Microsoft Office Click-to-Run
LocalPath                          : 
HttpCustomHeaders                  : 
CostPolicy                         : 0
SecurityFlags                      : 0
CallbackFreqPercent                : 
CallbackFreqSeconds                : 1
NoProgressTimeoutSeconds           : 1800
ForegroundPriority                 : True
BlockingMode                       : False
SecurityContext                    : 
NetworkToken                       : 
CorrelationVector                  : /ovgbBM6bkiHF+0p.12
DecryptionInfo                     : 
IntegrityCheckInfo                 : 
IntegrityCheckMandatory            : False
TotalSizeBytes                     : 70073445
DisallowOnCellular                 : False
HttpCustomAuthHeaders              : 
HttpAllowSecureToNonSecureRedirect : 
NonVolatile                        : False
```

## PARAMETERS

### -Uri

The remote URI path of the resource to download.

```yaml
Type: Uri
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, -WarningVariable, and -ProgressAction. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### DODownloadProperties

This cmdlet returns downloads and their properties from [DODownloadProperty](https://learn.microsoft.com/en-us/windows/win32/api/deliveryoptimization/ne-deliveryoptimization-dodownloadproperty).

## RELATED LINKS

[Invoke-DORequest](Invoke-DORequest.md)
