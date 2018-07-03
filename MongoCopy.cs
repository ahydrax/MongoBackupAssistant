using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using MongoDB.Driver;
using ShellProgressBar;

namespace MongoBackupAssistant
{
    internal static class MongoCopy
    {
        public static void Copy(CopyOptions options)
        {

            var progressBarOptions = new ProgressBarOptions
            {
                ProgressCharacter = '#'
            };

            using (var pBar = new ProgressBar(2, $"Copying db from {options.FromDatabase} to {options.ToDatabase}", progressBarOptions))
            {
                DumpDatabase(options, pBar, progressBarOptions);
                pBar.Tick();

                RestoreDatabase(options, pBar, progressBarOptions);
                pBar.Tick();
            }
        }

        private static void DumpDatabase(CopyOptions options, ProgressBar pBar, ProgressBarOptions progressBarOptions)
        {
            var dumpCommandLineBase =
                $"--host {options.FromHost} --port {options.FromPort} --db {options.FromDatabase} --query \"{options.Query}\" --gzip --out {options.Path}";

            var uri = MongoUrl.Create($"mongodb://{options.FromHost}:{options.FromPort}/{options.FromDatabase}");
            var client = new MongoClient(uri);
            var db = client.GetDatabase(uri.DatabaseName);

            var collectionNames = db.ListCollectionNames().ToList();

            using (var childPBar = pBar.Spawn(collectionNames.Count, "Dumping...", progressBarOptions))
            {
                foreach (var collectionName in collectionNames)
                {
                    childPBar.Tick($"Dumping {collectionName}");

                    var dumpCommandLine = $"{dumpCommandLineBase} --collection {collectionName} ";

                    var process = Process.Start(new ProcessStartInfo(options.MongoDumpBinary, dumpCommandLine)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    });

                    if (process == null)
                    {
                        throw new Exception($"Failed to start {dumpCommandLine}");
                    }

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Failed to dump collection {collectionName}");
                    }
                }
            }
        }

        private static void RestoreDatabase(CopyOptions options, ProgressBar pBar, ProgressBarOptions progressBarOptions)
        {
            var path = Path.Combine(options.Path, options.FromDatabase);
            var fileNames = Directory.GetFiles(path, "*.bson.gz");
            var restoreCommandLineBase = $"--host {options.ToHost} --port {options.ToPort} --db {options.ToDatabase} --gzip";
            using (var childPBar = pBar.Spawn(fileNames.Length, "Restoring...", progressBarOptions))
            {
                foreach (var archiveFullPath in fileNames)
                {
                    childPBar.Tick($"Restoring {archiveFullPath}");
                    var archiveName = Path.GetFileName(archiveFullPath);

                    var restoreCommandline = $"{restoreCommandLineBase} {archiveFullPath}";

                    var process = Process.Start(new ProcessStartInfo(options.MongoRestoreBinary, restoreCommandline)
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        StandardOutputEncoding = Encoding.UTF8
                    });

                    if (process == null)
                    {
                        throw new Exception($"Failed to start {restoreCommandline}");
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
