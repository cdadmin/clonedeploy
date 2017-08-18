namespace CloneDeploy_Entities.DTOs.ClientPartition
{
    public class ClientVolumeGroupHelper
    {
        public long AgreedPvSizeBlk { get; set; }
        public bool HasLv { get; set; }
        public bool IsFusion { get; set; }
        public long MinSizeBlk { get; set; }
        public string Name { get; set; }
        public string Pv { get; set; }
        public string Uuid { get; set; }
    }
}