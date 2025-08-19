---
document type: cmdlet
external help file: PSDODownloader-Help.xml
HelpUri: ''
Locale: en-AU
Module Name: PSDODownloader
ms.date: 08/19/2025
PlatyPS schema version: 2024-05-01
title: Invoke-DORequest
---

# Invoke-DORequest

## SYNOPSIS

Download a file using the Delivery Optimization service.

## SYNTAX

```powershell
Invoke-DORequest [[-Uri] <uri>] [-ContentId <string>] [-OutFile <string>] [-Ranges <int[]>]
 [-Caller <string>] [-TimeoutSec <int>] [<CommonParameters>]
```

## DESCRIPTION

The `Invoke-DORequest` cmdlet requests a file download from the Delivery Optimization service.
The download can be streamed and deleted after completion, or written to a provided path.
Byte ranges can also be requested, instead of the whole file.
No results are returned.

## EXAMPLES

### Example 1

Download a file using streaming and delete it. This may fail with write errors (`0x80190193 CBufferedWriteData::Write`).

```powershell
PS C:\> Invoke-DORequest -Uri http://dl.delivery.mp.microsoft.com/filestreamingservice/files/52fa8751-747d-479d-8f22-e32730cc0eb1
```

### Example 2

Download a file to a local path.

```powershell
PS C:\> Invoke-DORequest -Uri http://dl.delivery.mp.microsoft.com/filestreamingservice/files/52fa8751-747d-479d-8f22-e32730cc0eb1 -OutFile download.exe
```

### Example 3

Download parts of a file using [byte ranges](https://learn.microsoft.com/en-us/windows/win32/api/deliveryoptimization/ns-deliveryoptimization-do_download_range).

```powershell
PS C:\> Invoke-DORequest -Uri http://dl.delivery.mp.microsoft.com/filestreamingservice/files/52fa8751-747d-479d-8f22-e32730cc0eb1 -OutFile download.exe -Ranges 10,65536,131072,65536
```

## PARAMETERS

### -Caller

The caller display name, also known as `PredefinedCallerApplication`.

```yaml
Type: System.String
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -ContentId

The unique content ID of the resource to download.

```yaml
Type: System.String
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -OutFile

The local path name to save the download file.

```yaml
Type: System.String
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Ranges

Pairs of [byte ranges](https://learn.microsoft.com/en-us/windows/win32/api/deliveryoptimization/ns-deliveryoptimization-do_download_range) to download from a file.
Formatted as a flat array.

```yaml
Type: System.Int32[]
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -TimeoutSec

Specifies how long the request can be pending before it times out. Enter a value in seconds. The default value is 60.

```yaml
Type: System.Int32
DefaultValue: ''
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: Named
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### -Uri

The remote URI path of the resource to download.

```yaml
Type: System.Uri
DefaultValue: None
SupportsWildcards: false
Aliases: []
ParameterSets:
- Name: (All)
  Position: 0
  IsRequired: false
  ValueFromPipeline: false
  ValueFromPipelineByPropertyName: false
  ValueFromRemainingArguments: false
DontShow: false
AcceptedValues: []
HelpMessage: ''
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable,
-InformationAction, -InformationVariable, -OutBuffer, -OutVariable, -PipelineVariable,
-ProgressAction, -Verbose, -WarningAction, and -WarningVariable. For more information, see
[about_CommonParameters](https://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

### None <!-- markdownlint-disable MD024 -->

## RELATED LINKS

- [Get-DORequests](Get-DORequests.md)
