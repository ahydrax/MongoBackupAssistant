using CommandLine;

namespace MongoBackupAssistant
{
    [Verb("restore")]
    public class RestoreOptions
    {
        [Option('b', "restoreBin", HelpText = "Path to mongodump binary", Required = true)]
        public string MongoRestoreBinary { get; set; }

        [Option('i', "input", HelpText = "Backup path", Required = true)]
        public string InputPath { get; set; }

        [Option('u', "uri", HelpText = "Connection string", Required = true)]
        public string Uri { get; set; }

        [Option("drop")]
        public bool Drop { get; set; }
    }
}
