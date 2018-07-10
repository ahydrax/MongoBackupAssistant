using CommandLine;

namespace MongoBackupAssistant
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Parser.Default.ParseArguments<DumpOptions, RestoreOptions, CopyOptions>(args)
                .WithParsed<DumpOptions>(Mongo.Dump)
                .WithParsed<RestoreOptions>(Mongo.Restore)
                .WithParsed<CopyOptions>(Mongo.Copy);
        }
    }
}
