using CommandLine;

namespace MongoBackupAssistant
{
    public static class Program
    {
        public static void Main(string[] args)
            => Parser.Default.ParseArguments<DumpOptions, RestoreOptions, CopyOptions>(args)
                .WithParsed<DumpOptions>(MongoDump.Dump)
                .WithParsed<RestoreOptions>(MongoRestore.Restore)
                .WithParsed<CopyOptions>(MongoCopy.Copy);
    }
}
