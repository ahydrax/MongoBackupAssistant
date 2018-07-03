using CommandLine;

namespace MongoBackupAssistant
{
    [Verb("copy")]
    public class CopyOptions
    {
        [Option("dumpBin", Required = true)]
        public string MongoDumpBinary { get; set; }

        [Option("restoreBin", Required = true)]
        public string MongoRestoreBinary { get; set; }

        [Option("fromHost", Required = true)]
        public string FromHost { get; set; }

        [Option("fromPort", Default = 27017)]
        public int FromPort { get; set; }

        [Option("fromDb", Required = true)]
        public string FromDatabase { get; set; }

        [Option("toHost", Required = true)]
        public string ToHost { get; set; }

        [Option("toPort", Default = 27017)]
        public int ToPort { get; set; }

        [Option("toDb", Required = true)]
        public string ToDatabase { get; set; }

        [Option("path", Required = true)]
        public string Path { get; set; }

        [Option("query")]
        public string Query { get; set; }
    }
}
