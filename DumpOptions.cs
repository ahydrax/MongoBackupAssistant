using CommandLine;

namespace MongoBackupAssistant
{
    [Verb("dump")]
    public class DumpOptions
    {
        [Option('b', "dumpBinaryPath", HelpText = "Path to mongodump binary", Required = true)]
        public string MongoDumpPath { get; set; }

        [Option('q', "queryFile", HelpText = "Path to query file", Required = true)]
        public string QueryFile { get; set; }

        [Option('p', "prefix", HelpText = "Backup archives prefix name", Required = true)]
        public string PrefixName { get; set; }

        [Option('o', "out", HelpText = "Backup path", Required = true)]
        public string OutputPath { get; set; }
        
        [Option('h', "host", HelpText = "Database host", Required = true)]
        public string Host { get; set; }

        [Option('P', "port", HelpText = "Database port", Required = false, Default = 27017)]
        public int Port { get; set; }

        [Option('n', "name", HelpText = "Database name", Required = true)]
        public string DatabaseName { get; set; }
    }
}
