namespace CloneDeploy_Entities.DTOs.ClientPartition
{
    public class ExtendedPartitionHelper
    {
        public ExtendedPartitionHelper()
        {
            AgreedSizeBlk = 0;
            HasLogical = false;
            IsOnlySwap = false;
            LogicalCount = 0;
            MinSizeBlk = 0;
        }

        public long AgreedSizeBlk { get; set; }
        public bool HasLogical { get; set; }
        public bool IsOnlySwap { get; set; }
        public int LogicalCount { get; set; }
        public long MinSizeBlk { get; set; }
    }
}