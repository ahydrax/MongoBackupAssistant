using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MongoBackupAssistant
{
    internal static partial class Mongo
    {
        public static void Restore(RestoreOptions options)
        {
            var fileNames = Directory.GetFiles(options.InputPath, "*.bson.gz");
            var restoreCommandLineBase = $"--host {options.Host} --port {options.Port} --db {options.DatabaseName} --gzip";

            if (options.Drop)
            {
                restoreCommandLineBase += $" --drop";
            }

            for (var i = 0; i < fileNames.Length; i++)
            {
                var bsonFullPath = fileNames[i];
                var collectionName = Path.GetFileNameWithoutExtension(bsonFullPath);
                Console.WriteLine($"[{i + 1}/{fileNames.Length}] Restoring {collectionName} from {bsonFullPath}");
                var archiveName = Path.GetFileName(bsonFullPath);

                var restoreCommandline = $"{restoreCommandLineBase} {bsonFullPath}";

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
