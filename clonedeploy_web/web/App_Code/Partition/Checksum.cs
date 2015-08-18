namespace Partition
{
    public class HdChecksum
    {
        public FileChecksum[] Fc { get; set; }
        public string HdNumber { get; set; }
        public string Path { get; set; }
    }

    public class FileChecksum
    {
        public string Checksum { get; set; }
        public string FileName { get; set; }
    }
}