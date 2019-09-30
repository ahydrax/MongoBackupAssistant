using CommandLine;

namespace MongoBackupAssistant
{
    [Verb("dump")]
    public class DumpOptions
    {
        [Option('b', "dumpBin", HelpText = "Path to mongodump binary", Required = true)]
        public string MongoDumpBinary { get; set; }

        [Option('q', "query", HelpText = "Path to query file", Required = true)]
        public string Query { get; set; }

        [Option('o', "out", HelpText = "Backup path", Required = true)]
        public string OutputPath { get; set; }

        [Option('u', "uri", HelpText = "Connection string", Required = true)]
        public string Uri { get; set; }
    }
}
