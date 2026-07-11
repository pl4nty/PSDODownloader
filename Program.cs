using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text;

// Sample code to showcase usage of Delivery Optimization client's COM API via C# Interop.
// COM API is documented here: https://learn.microsoft.com/en-us/windows/win32/delivery_optimization/do-reference

namespace DODownloader
{
    internal class Program
    {
        internal class Options
        {
            public enum Actions
            {
                None,
                EnumerateDownloads,
            }

            public Actions Action = Actions.None;
            public string Url;
            public string OutputFilePath;
            public DODownloadRanges DownloadRanges;
            public bool IsStreamDownload => string.IsNullOrEmpty(OutputFilePath);

            public void SetRangesIfEmpty()
            {
                if (DownloadRanges != null)
                {
                    return;
                }

                if (IsStreamDownload)
                {
                    // Streaming download requires a range to specified (upto and including Win11 22H2).
                    // Range of offset = 0, length = max-uint64 indicates a full file range request.
                    DownloadRanges = new DODownloadRanges(new[] { 0ul, ulong.MaxValue });
                }
                else
                {
                    // Full file download is indicated with empty/zero ranges object
                    DownloadRanges = new DODownloadRanges();
                }
            }

            public static bool TryParseArgs(string[] args, out Options options)
            {
                options = new Options();
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals("--url"))
                    {
                        if ((i + 1) >= args.Length) return false;
                        if (string.IsNullOrWhiteSpace(args[i + 1])) return false;
                        options.Url = args[++i];
                    }
                    else if (args[i].Equals("--output-file-path"))
                    {
                        if ((i + 1) >= args.Length) return false;
                        options.OutputFilePath = args[++i];
                    }
                    else if (args[i].Equals("--ranges"))
                    {
                        if ((i + 1) >= args.Length) return false;
                        try
                        {
                            options.DownloadRanges = ParseDownloadRanges(args[++i]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error parsing ranges argument: {ex.Message}");
                            return false;
                        }
                    }
                    else if (args[i].Equals("--enumerate"))
                    {
                        options.Action = Actions.EnumerateDownloads;
                    }
                    else
                    {
                        Console.WriteLine($"Unknown cmdline option '{args[i]}'");
                        return false;
                    }
                }
                return (options.Action == Actions.EnumerateDownloads) || (options.Url != null);
            }

            private static DODownloadRanges ParseDownloadRanges(string arg)
            {
                string[] offsetsAndLengths = arg.Split(',');
                var offsetLengths = new ulong[offsetsAndLengths.Length];
                int i = 0;
                foreach (var val in offsetsAndLengths)
                {
                    offsetLengths[i++] = Convert.ToUInt64(val);
                }
                return new DODownloadRanges(offsetLengths);
            }
        }

        internal static object GetDownloadProperty(IDODownload download, DODownloadProperty downloadProperty)
        {
            try
            {
                download.GetProperty(downloadProperty, out object value);
                return value;
            }
            catch (COMException ce)
            {
                Console.WriteLine($"Get property {downloadProperty} failed with error {ce.HResult:x}");
                return null;
            }
        }
    }

    internal class PSTextWriter : TextWriter
    {
        Cmdlet cmdlet;

        internal PSTextWriter(Cmdlet cmdlet)
        {
            this.cmdlet = cmdlet;
        }

        public override Encoding Encoding => Encoding;

        public override void WriteLine(string value)
        {
            cmdlet.WriteVerbose(value);
        }
    }

    public class DODownloadProperties {
        public string Id { get; set; }
        public string Uri { get; set; }
        public string ContentId { get; set; }
        public string DisplayName { get; set; }
        public string LocalPath { get; set; }
        public string HttpCustomHeaders { get; set; }
        public string CostPolicy { get; set; }
        public string SecurityFlags { get; set; }
        public string CallbackFreqPercent { get; set; }
        public string CallbackFreqSeconds { get; set; }
        public string NoProgressTimeoutSeconds { get; set; }
        public string ForegroundPriority { get; set; }
        public string BlockingMode { get; set; }
        public string CallbackInterface { get; set; }
        public string StreamInterface { get; set; }
        public string SecurityContext { get; set; }
        public string NetworkToken { get; set; }
        public string CorrelationVector { get; set; }
        public string DecryptionInfo { get; set; }
        public string IntegrityCheckInfo { get; set; }
        public string IntegrityCheckMandatory { get; set; }
        public string TotalSizeBytes { get; set; }
        public string DisallowOnCellular { get; set; }
        public string HttpCustomAuthHeaders { get; set; }
        public string HttpAllowSecureToNonSecureRedirect { get; set; }
        public string NonVolatile { get; set; }
    }

    [Cmdlet(VerbsLifecycle.Invoke, "Request")]
    public class ExecDownload : PSCmdlet, IDisposable
    {
        [Parameter(Position = 0)]
        public Uri Uri { get; set; }

        [Parameter]
        public string ContentId { get; set; }

        [Parameter]
        public string OutFile { get; set; }

        [Parameter]
        public int[] Ranges { get; set; }

        [Parameter]
        public string Caller { get; set; }

        [Parameter(Mandatory = false)]
        [PSDefaultValue(Value = 60)]
        public int TimeoutSec { get; set; } = 60;

        [Parameter]
        public SwitchParameter Background { get; set; }

        TextWriter standardOut;

        protected override void BeginProcessing()
        {
            if (string.IsNullOrEmpty(Uri?.AbsoluteUri) && string.IsNullOrEmpty(ContentId))
            {
                ThrowTerminatingError(
                    new ErrorRecord(
                        new ArgumentException("At least one of 'Uri' or 'ContentId' must be provided."),
                        "MissingParameter",
                        ErrorCategory.InvalidArgument,
                        null
                    )
                );
            }

            standardOut = Console.Out;
            Console.SetOut(new PSTextWriter(this));
            Program.Options options = new Program.Options()
            {
                Action = Program.Options.Actions.None,
                Url = Uri?.OriginalString,
                OutputFilePath = string.IsNullOrEmpty(OutFile) ? null : Path.GetFullPath(OutFile),
                DownloadRanges = Ranges != null ? new DODownloadRanges(Array.ConvertAll(Ranges, x => (ulong)x)) : null
            };

            options.SetRangesIfEmpty();

            var factory = new DODownloadFactory(Caller ?? "PSDODownloader");
            var file = new DOFile(options.Url, ContentId);

            DODownload download = null;
            ConsoleCancelEventHandler cancelHandler = (sender, e) =>
            {
                e.Cancel = true;
                download?.Abort();
            };
            Console.CancelKeyPress += cancelHandler;
            SequentialStreamReceiver downloadDataSink = null;
            try
            {
                if (options.IsStreamDownload)
                {
                    downloadDataSink = new SequentialStreamReceiver();
                    download = factory.CreateDownloadWithStreamOutput(file, downloadDataSink);
                }
                else
                {
                    if (File.Exists(options.OutputFilePath))
                    {
                        File.Delete(options.OutputFilePath);
                    }
                    download = factory.CreateDownloadWithFileOutput(file, options.OutputFilePath);
                }

                if(Background.IsPresent)
                {
                    download.SetBackground();
                }
                else
                {
                    download.SetForeground();
                }

                download.StartAndWaitUntilTransferred(options.DownloadRanges, completionTimeSecs: TimeoutSec);
                // Here, we can do something more, like query stats from download.
                // Then let DO client know that we are done with this download object.
                download.Finalize2();

                if (options.IsStreamDownload)
                {
                    Console.WriteLine($"Download completed, received {downloadDataSink.TotalBytesReceived} bytes via"
                        + $" {downloadDataSink.TotalCallsReceived} stream write calls");
                }
                else
                {
                    var fileSize = new FileInfo(options.OutputFilePath).Length;
                    Console.WriteLine($"Download completed, output file size {fileSize} bytes at {options.OutputFilePath}");
                }
            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, $"Download failed. Exception: hr: {ex.HResult:X}, {ex.Message}\n{ex.StackTrace}", ErrorCategory.NotSpecified, null));
                download?.Abort();
            }
        }

        public void Dispose()
        {
            Console.SetOut(standardOut);
        }
    }

    // Enumerate existing downloads with an optionally filtering by URL.
    // Future: Could support other filtering on other properties.
    [Cmdlet(VerbsCommon.Get, "Requests")]
    [OutputType(typeof(DODownloadProperties))]
    public class ExecEnumeration : PSCmdlet, IDisposable
    {
        [Parameter]
        public Uri Uri { get; set; }

        TextWriter standardOut;

        protected override void BeginProcessing()
        {
            standardOut = Console.Out;
            Console.SetOut(new PSTextWriter(this));
            var factory = new DODownloadFactory("PSDODownloader");
            List<IDODownload> downloads = Uri == null ?
                factory.EnumerateDownloads() :
                factory.EnumerateDownloads(DODownloadProperty.Uri, Uri.OriginalString);
            Console.WriteLine($"Enumeration found {downloads.Count} download(s).");

            foreach (var download in downloads)
            {
                WriteObject(new
                {
                    Id = Program.GetDownloadProperty(download, DODownloadProperty.Id),
                    Uri = Program.GetDownloadProperty(download, DODownloadProperty.Uri),
                    ContentId = Program.GetDownloadProperty(download, DODownloadProperty.ContentId),
                    DisplayName = Program.GetDownloadProperty(download, DODownloadProperty.DisplayName),
                    LocalPath = Program.GetDownloadProperty(download, DODownloadProperty.LocalPath),
                    HttpCustomHeaders = Program.GetDownloadProperty(download, DODownloadProperty.HttpCustomHeaders),
                    CostPolicy = Program.GetDownloadProperty(download, DODownloadProperty.CostPolicy),
                    SecurityFlags = Program.GetDownloadProperty(download, DODownloadProperty.SecurityFlags),
                    CallbackFreqPercent = Program.GetDownloadProperty(download, DODownloadProperty.CallbackFreqPercent),
                    CallbackFreqSeconds = Program.GetDownloadProperty(download, DODownloadProperty.CallbackFreqSeconds),
                    NoProgressTimeoutSeconds = Program.GetDownloadProperty(download, DODownloadProperty.NoProgressTimeoutSeconds),
                    ForegroundPriority = Program.GetDownloadProperty(download, DODownloadProperty.ForegroundPriority),
                    BlockingMode = Program.GetDownloadProperty(download, DODownloadProperty.BlockingMode),
                    // CallbackInterface = Program.GetDownloadProperty(download, DODownloadProperty.CallbackInterface),
                    // StreamInterface = Program.GetDownloadProperty(download, DODownloadProperty.StreamInterface),
                    SecurityContext = Program.GetDownloadProperty(download, DODownloadProperty.SecurityContext),
                    NetworkToken = Program.GetDownloadProperty(download, DODownloadProperty.NetworkToken),
                    CorrelationVector = Program.GetDownloadProperty(download, DODownloadProperty.CorrelationVector),
                    DecryptionInfo = Program.GetDownloadProperty(download, DODownloadProperty.DecryptionInfo),
                    IntegrityCheckInfo = Program.GetDownloadProperty(download, DODownloadProperty.IntegrityCheckInfo),
                    IntegrityCheckMandatory = Program.GetDownloadProperty(download, DODownloadProperty.IntegrityCheckMandatory),
                    TotalSizeBytes = Program.GetDownloadProperty(download, DODownloadProperty.TotalSizeBytes),
                    DisallowOnCellular = Program.GetDownloadProperty(download, DODownloadProperty.DisallowOnCellular),
                    HttpCustomAuthHeaders = Program.GetDownloadProperty(download, DODownloadProperty.HttpCustomAuthHeaders),
                    HttpAllowSecureToNonSecureRedirect = Program.GetDownloadProperty(download, DODownloadProperty.HttpAllowSecureToNonSecureRedirect),
                    NonVolatile = Program.GetDownloadProperty(download, DODownloadProperty.NonVolatile),
                });
            }
        }

        public void Dispose()
        {
            Console.SetOut(standardOut);
        }
    }
}
