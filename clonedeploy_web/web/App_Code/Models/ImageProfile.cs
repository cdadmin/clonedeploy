using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("image_profiles")]
    public class ImageProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_id", Order = 1)]
        public int Id { get; set; }

        [Column("profile_name", Order = 2)]
        public string Name { get; set; }

        [Column("profile_description", Order = 3)]
        public string Description { get; set; }

        [Column("image_id", Order = 4)]
        public int ImageId { get; set; }

        [Column("profile_kernel", Order = 5)]
        public string Kernel { get; set; }

        [Column("profile_boot_image", Order = 6)]
        public string BootImage { get; set; }

        [Column("profile_kernel_arguments", Order = 7)]
        public string KernelArguments { get; set; }

        [Column("skip_core_download", Order = 8)]
        public int SkipCore { get; set; }

        [Column("skip_set_clock", Order = 9)]
        public int SkipClock { get; set; }

        [Column("task_completed_action", Order = 10)]
        public string TaskCompletedAction { get; set; }

        [Column("remove_gpt_structures", Order = 11)]
        public int RemoveGPT { get; set; }

        [Column("skip_volume_shrink", Order = 12)]
        public int SkipShrinkVolumes { get; set; }

        [Column("skip_lvm_shrink", Order = 13)]
        public int SkipShrinkLvm { get; set; }

        [Column("skip_volume_expand", Order = 14)]
        public int SkipExpandVolumes { get; set; }

        [Column("fix_bcd", Order = 15)]
        public int FixBcd { get; set; }

        [Column("fix_bootloader", Order = 16)]
        public int FixBootloader { get; set; }

        [Column("partition_method", Order = 17)]
        public string PartitionMethod { get; set; }

        [Column("force_dynamic_partitions", Order = 18)]
        public int ForceDynamicPartitions { get; set; }

        [Column("custom_partition_script", Order = 19)]
        public string CustomPartitionScript { get; set; }

        [Column("compression_algorithm", Order = 20)]
        public string Compression { get; set; }

        [Column("compression_level", Order = 21)]
        public string CompressionLevel { get; set; }

        [Column("custom_image_schema", Order = 22)]
        public string CustomSchema { get; set; }

        [Column("custom_upload_schema", Order = 23)]
        public string CustomUploadSchema { get; set; }

        [Column("upload_schema_only", Order = 24)]
        public int UploadSchemaOnly { get; set; }

        [Column("multicast_sender_arguments", Order = 25)]
        public string SenderArguments { get; set; }

        [Column("multicast_receiver_arguments", Order = 26)]
        public string ReceiverArguments { get; set; }

        [Column("web_cancel", Order = 27)]
        public int WebCancel { get; set; }

        [Column("change_name", Order = 28)]
        public int ChangeName { get; set; }

        [Column("osx_target_volume", Order = 29)]
        public string OsxTargetVolume { get; set; }

        [Column("osx_install_munki", Order = 30)]
        public int OsxInstallMunki { get; set; }

        [Column("munki_repo_url", Order = 31)]
        public string MunkiRepoUrl { get; set; }

        [NotMapped]
        public virtual Models.Image Image { get; set; }

   
    }
}

