using System.IO;
using MongoDB.Driver;

namespace MongoBackupAssistant
{
    internal static partial class Mongo
    {
        public static void Copy(CopyOptions options)
        {
            var dumpOptions = new DumpOptions
            {
                Uri = options.FromUri,
                Query = options.Query,
                OutputPath = options.Path,
                MongoDumpBinary = options.MongoDumpBinary
            };
            Dump(dumpOptions);

            var dataBaseName = MongoUrl.Create(options.FromUri).DatabaseName;
            var restoreOptions = new RestoreOptions
            {
                Uri = options.ToUri,
                InputPath = Path.Combine(options.Path, dataBaseName),
                MongoRestoreBinary = options.MongoRestoreBinary
            };
            Restore(restoreOptions);
        }
    }
}
