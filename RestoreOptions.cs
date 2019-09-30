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

        [Option('h', "host", HelpText = "Database host", Required = true)]
        public string Host { get; set; }

        [Option('P', "port", HelpText = "Database port", Required = false, Default = 27017)]
        public int Port { get; set; }

        [Option('n', "name", HelpText = "Database name", Required = true)]
        public string DatabaseName { get; set; }

        [Option('u', "user", HelpText = "Username", Required = false)]
        public string Username { get; set; }

        [Option('w', "pass", HelpText = "Password", Required = false)]
        public string Password { get; set; }

        [Option("drop")]
        public bool Drop { get; set; }
    }
}
