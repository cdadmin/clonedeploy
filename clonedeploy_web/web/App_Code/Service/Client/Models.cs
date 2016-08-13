using System.Collections.Generic;

namespace Services.Client
{
    public class WinPEMulticastList
    {
        public string Port { get; set; }
        public string Name { get; set; }
    }

    public class WinPEImageList
    {
        public string ImageId { get; set; }
        public string ImageName { get; set; }
    }

    public class WinPEProfileList
    {
        public string Count { get; set; }
        public string FirstProfileId { get; set; }
        public List<WinPEProfile> ImageProfiles { get; set; }      
    }

    public class WinPEProfile
    {
        public string ProfileId { get; set; }
        public string ProfileName { get; set; }
    }

    public class ImageList
    {
        public List<string> Images  { get; set; }
    }

    public class ImageProfileList
    {
        public string Count { get; set; }
        public string FirstProfileId { get; set; }
        public List<string> ImageProfiles { get; set; }         
    }

    public class MulticastList
    {
        public List<string> Multicasts { get; set; }
    }

    public class CheckIn
    {
        public string Result { get; set; }
        public string Message { get; set; }
        public string TaskArguments { get; set; }
    }

    public class SMB
    {
        public string SharePath { get; set; }
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class QueueStatus
    {
        public string Result { get; set; }
        public string Position { get; set; }
    }

    public class HardDriveSchema
    {
        public string IsValid { get; set; }
        public string Message { get; set; }
        public int SchemaHdNumber { get; set; }
        public int PhysicalPartitionCount { get; set; }
        public string PartitionType { get; set; }
        public string BootPartition { get; set; }
        public string Guid { get; set; }
        public string UsesLvm { get; set; }
        public List<PhysicalPartition> PhysicalPartitions { get; set; }
    }


    public class PhysicalPartition
    {
        public string Number { get; set; }
        public string PartcloneFileSystem { get; set; }
        public string Compression { get; set; }
        public string FileSystem { get; set; }
        public string Uuid { get; set; }
        public string Guid { get; set; }
        public string Type { get; set; }
        public string Prefix { get; set; }
        public string EfiBootLoader { get; set; }
        public string ImageType { get; set; }
        public VolumeGroup VolumeGroup { get; set; }
    }

    public class VolumeGroup
    {
        public string Name { get; set; }
        public int LogicalVolumeCount { get; set; }
        public List<LogicalVolume> LogicalVolumes { get; set; } 
    }

    public class LogicalVolume
    {
        public string Name { get; set; }
        public string PartcloneFileSystem { get; set; }
        public string Compression { get; set; }
        public string FileSystem { get; set; }
        public string Uuid { get; set; }
        public string ImageType { get; set; }
    }

    public class FileFolderCopySchema
    {
        public string Count { get; set; }
        public List<FileFolderCopy> FilesAndFolders { get; set; } 
    }

    public class FileFolderCopy
    {
        public string SourcePath { get; set; }
        public string DestinationFolder { get; set; }
        public string DestinationPartition { get; set; }
        public string FolderCopyType { get; set; }
    }

    public class ProxyReservation
    {
        public string NextServer { get; set; }
        public string BootFile { get; set; }
        public string BcdFile { get; set; }
    }
}
