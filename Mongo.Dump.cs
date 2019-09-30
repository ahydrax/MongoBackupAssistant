using System;
using System.Diagnostics;
using System.IO;
using MongoDB.Driver;

namespace MongoBackupAssistant
{
    internal static partial class Mongo
    {
        public static void Dump(DumpOptions options)
        {
            var uri = MongoUrl.Create(options.Uri);
            var targetPath = Path.Combine(options.OutputPath, uri.DatabaseName);
            var dumpCommandLineBase = $"--uri {options.Uri} --gzip --out {options.OutputPath}";

            if (!string.IsNullOrWhiteSpace(options.Query))
            {
                dumpCommandLineBase += $" --query \"{options.Query}\"";
            }

            var client = new MongoClient(uri);
            var db = client.GetDatabase(uri.DatabaseName);

            var collectionNames = db.ListCollectionNames().ToList();

            for (var i = 0; i < collectionNames.Count; i++)
            {
                var collectionName = collectionNames[i];
                Console.WriteLine($"[{i + 1}/{collectionNames.Count}] Dumping {collectionName} to {targetPath}");

                var dumpCommandLine = $"{dumpCommandLineBase} --collection {collectionName} ";

                var process = Process.Start(new ProcessStartInfo(options.MongoDumpBinary, dumpCommandLine)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = Console.OutputEncoding
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
}
