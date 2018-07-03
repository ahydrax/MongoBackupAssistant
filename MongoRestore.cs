using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using ShellProgressBar;

namespace MongoBackupAssistant
{
    internal static class MongoRestore
    {
        public static void Restore(RestoreOptions options)
        {
            var commandLineBase = $"--host {options.Host} --port {options.Port} --db {options.DatabaseName} --gzip";

            var fileNames = Directory.GetFiles(options.InputPath, "*.bson.gz");

            var progressBarOptions = new ProgressBarOptions
            {
                ProgressCharacter = '|'
            };

            using (var pBar = new ProgressBar(fileNames.Length, "Starting...", progressBarOptions))
            {
                foreach (var archiveFullPath in fileNames)
                {
                    pBar.Tick($"Restoring {archiveFullPath}");
                    var archiveName = Path.GetFileName(archiveFullPath);

                    var commandLine = $"{commandLineBase} {archiveFullPath}";

                    var process = Process.Start(new ProcessStartInfo(options.MongoRestorePath, commandLine)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    });

                    if (process == null)
                    {
                        throw new Exception($"Failed to start {commandLine}");
                    }

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Failed to restore collection {archiveName}");
                    }
                }
            }
        }
    }
}
