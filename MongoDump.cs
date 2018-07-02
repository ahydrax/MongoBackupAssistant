using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using MongoDB.Driver;
using ShellProgressBar;

namespace MongoBackupAssistant
{
    internal static class MongoDump
    {
        public static void Dump(DumpOptions options)
        {
            var commandLineBase = $"--host {options.Host} --port {options.Port} --db {options.DatabaseName}";
            
            var uri = MongoUrl.Create($"mongodb://{options.Host}:{options.Port}/{options.DatabaseName}");
            var client = new MongoClient(uri);
            var db = client.GetDatabase(uri.DatabaseName);

            var collectionNames = db.ListCollectionNames().ToList();

            var progressBarOptions = new ProgressBarOptions
            {
                ProgressCharacter = '|',
                ProgressBarOnBottom = true
            };

            using (var pBar = new ProgressBar(collectionNames.Count, "Starting...", progressBarOptions))
            {
                foreach (var collectionName in collectionNames)
                {
                    pBar.Tick($"Dumping {collectionName}");

                    var archiveName = $"{options.PrefixName}.{collectionName}.archive";
                    var archiveFullPath = Path.Combine(options.OutputPath, archiveName);
                    var commandLine = $"{commandLineBase} --collection {collectionName} --queryFile {options.QueryFile}  --out {options.OutputPath} --gzip";

                    var process = Process.Start(new ProcessStartInfo(options.MongoDumpPath, commandLine)
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
                        throw new Exception($"Failed to dump collection {collectionName}");
                    }
                }
            }
        }
    }
}
