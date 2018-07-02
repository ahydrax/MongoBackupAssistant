using CommandLine;

namespace MongoBackupAssistant
{
    public static class Program
    {
        public static void Main(string[] args)
            => Parser.Default.ParseArguments<DumpOptions, RestoreOptions>(args)
                .WithParsed<DumpOptions>(x => MongoDump.Dump(x))
                .WithParsed<RestoreOptions>(x => MongoRestore.Restore(x));
    }
}
