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

        [Option('h', "host", HelpText = "Database host", Required = true)]
        public string Host { get; set; }

        [Option('P', "port", HelpText = "Database port", Required = false, Default = 27017)]
        public int Port { get; set; }

        [Option('n', "name", HelpText = "Database name", Required = true)]
        public string DatabaseName { get; set; }

        [Option('u', "user", HelpText = "Username", Required = false)]
        public string Username { get; set; }

        [Option('w' ,"pass", HelpText = "Password", Required = false)]
        public string Password { get; set; }
    }
}
