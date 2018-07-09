using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using MongoDB.Driver;

namespace MongoBackupAssistant
{
    internal static class MongoCopy
    {
        public static void Copy(CopyOptions options)
        {
            DumpDatabase(options);
            RestoreDatabase(options);
        }

        private static void DumpDatabase(CopyOptions options)
        {
            var dumpCommandLineBase =
                $"--host {options.FromHost} --port {options.FromPort} --db {options.FromDatabase} --gzip --out {options.Path}";

            if (options.Query != null)
            {
                dumpCommandLineBase += $" --query \"{options.Query}\"";
            }

            var uri = MongoUrl.Create($"mongodb://{options.FromHost}:{options.FromPort}/{options.FromDatabase}");
            var client = new MongoClient(uri);
            var db = client.GetDatabase(uri.DatabaseName);

            var collectionNames = db.ListCollectionNames().ToList();

            foreach (var collectionName in collectionNames)
            {
                Console.WriteLine($"Dumping {collectionName}");

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

        private static void RestoreDatabase(CopyOptions options)
        {
            var path = Path.Combine(options.Path, options.FromDatabase);
            var fileNames = Directory.GetFiles(path, "*.bson.gz");
            var restoreCommandLineBase = $"--host {options.ToHost} --port {options.ToPort} --db {options.ToDatabase} --gzip";

            if (options.Drop)
            {
                restoreCommandLineBase += $" --drop";
            }

            foreach (var archiveFullPath in fileNames)
            {
                Console.WriteLine($"Restoring {archiveFullPath}");
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
