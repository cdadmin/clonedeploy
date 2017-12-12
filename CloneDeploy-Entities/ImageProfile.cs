using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("image_profiles")]
    public class ImageProfileEntity
    {
        [Column("profile_boot_image", Order = 6)]
        public string BootImage { get; set; }

        [Column("change_name", Order = 28)]
        public int ChangeName { get; set; }

        [Column("compression_algorithm", Order = 20)]
        public string Compression { get; set; }

        [Column("compression_level", Order = 21)]
        public string CompressionLevel { get; set; }

        [Column("custom_partition_script", Order = 19)]
        public string CustomPartitionScript { get; set; }

        [Column("custom_image_schema", Order = 22)]
        public string CustomSchema { get; set; }

        [Column("custom_upload_schema", Order = 23)]
        public string CustomUploadSchema { get; set; }

        [Column("profile_description", Order = 3)]
        public string Description { get; set; }

        [Column("fix_bcd", Order = 15)]
        public int FixBcd { get; set; }

        [Column("fix_bootloader", Order = 16)]
        public int FixBootloader { get; set; }

        [Column("force_dynamic_partitions", Order = 18)]
        public int ForceDynamicPartitions { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_id", Order = 4)]
        public int ImageId { get; set; }

        [Column("profile_kernel", Order = 5)]
        public string Kernel { get; set; }

        [Column("profile_kernel_arguments", Order = 7)]
        public string KernelArguments { get; set; }

        [Column("munki_auth_password", Order = 33)]
        public string MunkiAuthPassword { get; set; }

        [Column("munki_auth_username", Order = 32)]
        public string MunkiAuthUsername { get; set; }

        [Column("munki_repo_url", Order = 31)]
        public string MunkiRepoUrl { get; set; }

        [Column("profile_name", Order = 2)]
        public string Name { get; set; }

        [Column("osx_install_munki", Order = 30)]
        public int OsxInstallMunki { get; set; }

        [Column("osx_target_volume", Order = 29)]
        public string OsxTargetVolume { get; set; }

        [Column("partition_method", Order = 17)]
        public string PartitionMethod { get; set; }

        [Column("multicast_receiver_arguments", Order = 26)]
        public string ReceiverArguments { get; set; }

        [Column("remove_gpt_structures", Order = 11)]
        public int RemoveGPT { get; set; }

        [Column("multicast_sender_arguments", Order = 25)]
        public string SenderArguments { get; set; }

        [Column("skip_set_clock", Order = 9)]
        public int SkipClock { get; set; }

        [Column("skip_core_download", Order = 8)]
        public int SkipCore { get; set; }

        [Column("skip_volume_expand", Order = 14)]
        public int SkipExpandVolumes { get; set; }

        [Column("skip_lvm_shrink", Order = 13)]
        public int SkipShrinkLvm { get; set; }

        [Column("skip_volume_shrink", Order = 12)]
        public int SkipShrinkVolumes { get; set; }

        [Column("task_completed_action", Order = 10)]
        public string TaskCompletedAction { get; set; }

        [Column("upload_schema_only", Order = 24)]
        public int UploadSchemaOnly { get; set; }

        [Column("web_cancel", Order = 27)]
        public int WebCancel { get; set; }

        [Column("wim_enabled_multicast", Order = 34)]
        public int WimMulticastEnabled { get; set; }

        [Column("skip_nvram", Order = 35)]
        public int SkipNvramUpdate { get; set; }

        [Column("randomize_guids", Order = 36)]
        public int RandomizeGuids { get; set; }

        [Column("force_standard_efi", Order = 37)]
        public int ForceStandardEfi { get; set; }

        [Column("force_standard_legacy", Order = 38)]
        public int ForceStandardLegacy { get; set; }

        [Column("simple_upload_schema", Order = 39)]
        public int SimpleUploadSchema { get; set; }

        [Column("erase_partitions", Order = 40)]
        public int ErasePartitions { get; set; }
    }

    [NotMapped]
    public class ImageProfileWithImage : ImageProfileEntity
    {
        public ImageEntity Image { get; set; }
    }
}