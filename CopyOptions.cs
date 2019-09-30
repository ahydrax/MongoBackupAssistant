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

        [Option("fromUri", Required = true)]
        public string FromUri { get; set; }

        [Option("toUri", Required = true)]
        public string ToUri { get; set; }

        [Option("path", Required = true)]
        public string Path { get; set; }

        [Option("query")]
        public string Query { get; set; }

        [Option("drop")]
        public bool Drop { get; set; }
    }
}
