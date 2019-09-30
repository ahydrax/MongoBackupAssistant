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
            var targetPath = Path.Combine(options.OutputPath, options.DatabaseName);
            var dumpCommandLineBase = $"--host {options.Host} --port {options.Port} --db {options.DatabaseName} --gzip --out {options.OutputPath}";

            if (!string.IsNullOrEmpty(options.Username) && !string.IsNullOrEmpty(options.Password))
                dumpCommandLineBase += $"--username {options.Username} --password {options.Password}";

            if (!string.IsNullOrWhiteSpace(options.Query))
            {
                dumpCommandLineBase += $" --query \"{options.Query}\"";
            }

            var uri = MongoUrl.Create(GetConnectionString(options));
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

        private static string GetConnectionString(DumpOptions options)
        {
            if (!string.IsNullOrEmpty(options.Username) && !string.IsNullOrEmpty(options.Password))
                return $"mongodb://{options.Username}:{options.Password}@{options.Host}:{options.Port}/{options.DatabaseName}";
            return $"mongodb://{options.Host}:{options.Port}/{options.DatabaseName}";
        }
    }
}
