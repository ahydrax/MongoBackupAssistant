using CommandLine;

namespace MongoBackupAssistant
{
    [Verb("restore")]
    public class RestoreOptions
    {
        [Option('b', "restoreBinaryPath", HelpText = "Path to mongodump binary", Required = true)]
        public string MongoRestorePath { get; set; }

        [Option('p', "prefix", HelpText = "Backup archives prefix name", Required = true)]
        public string PrefixName { get; set; }

        [Option('i', "input", HelpText = "Backup path", Required = true)]
        public string InputPath { get; set; }

        [Option('h', "host", HelpText = "Database host", Required = true)]
        public string Host { get; set; }

        [Option('P', "port", HelpText = "Database port", Required = false, Default = 27017)]
        public int Port { get; set; }

        [Option('n', "name", HelpText = "Database name", Required = true)]
        public string DatabaseName { get; set; }
    }
}
