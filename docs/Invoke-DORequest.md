---
external help file: DODownloader.dll-Help.xml
Module Name: PSDODownloader
online version:
schema: 2.0.0
---

# Invoke-DORequest

## SYNOPSIS

Download a file using the Delivery Optimization service.

## SYNTAX

```powershell
Invoke-DORequest -Uri <Uri> [-OutFile <String>] [-Ranges <Int32[]>] [<CommonParameters>]
```

## DESCRIPTION

The `Invoke-DORequest` cmdlet requests a file download from the Delivery Optimization service.
The download can be streamed and deleted after completion, or written to a provided path. Byte ranges can also be requested, instead of the whole file.
No results are returned.

## EXAMPLES

### Example 1

Download a file using streaming and delete it.

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
PS C:\> Invoke-DORequest -Uri http://dl.delivery.mp.microsoft.com/filestreamingservice/files/52fa8751-747d-479d-8f22-e32730cc0eb1 -Ranges 10,65536,131072,65536
```

## PARAMETERS

### -OutFile

The local path name to save the download file.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Ranges

Pairs of [byte ranges](https://learn.microsoft.com/en-us/windows/win32/api/deliveryoptimization/ns-deliveryoptimization-do_download_range) to download from a file. Formatted as a flat array.

```yaml
Type: Int32[]
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Uri

The remote URI path of the resource to download.

```yaml
Type: Uri
Parameter Sets: (All)
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters

This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

### None

## OUTPUTS

### None <!-- markdownlint-disable MD024 -->

## NOTES

## RELATED LINKS

[Get-DORequests](Get-DORequests.md)
