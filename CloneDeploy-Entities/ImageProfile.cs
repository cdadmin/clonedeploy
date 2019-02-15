using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloneDeploy_Entities
{
    [Table("image_profiles")]
    public class ImageProfileEntity
    {
        [Column("profile_boot_image")]
        public string BootImage { get; set; }

        [Column("change_name")]
        public int ChangeName { get; set; }

        [Column("compression_algorithm")]
        public string Compression { get; set; }

        [Column("compression_level")]
        public string CompressionLevel { get; set; }

        [Column("custom_partition_script")]
        public string CustomPartitionScript { get; set; }

        [Column("custom_image_schema")]
        public string CustomSchema { get; set; }

        [Column("custom_upload_schema")]
        public string CustomUploadSchema { get; set; }

        [Column("profile_description")]
        public string Description { get; set; }

        [Column("fix_bcd")]
        public int FixBcd { get; set; }

        [Column("fix_bootloader")]
        public int FixBootloader { get; set; }

        [Column("force_dynamic_partitions")]
        public int ForceDynamicPartitions { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_profile_id")]
        public int Id { get; set; }

        [Column("image_id")]
        public int ImageId { get; set; }

        [Column("profile_kernel")]
        public string Kernel { get; set; }

        [Column("profile_kernel_arguments")]
        public string KernelArguments { get; set; }

        [Column("profile_name")]
        public string Name { get; set; }

        [Column("partition_method")]
        public string PartitionMethod { get; set; }

        [Column("multicast_receiver_arguments")]
        public string ReceiverArguments { get; set; }

        [Column("remove_gpt_structures")]
        public int RemoveGPT { get; set; }

        [Column("multicast_sender_arguments")]
        public string SenderArguments { get; set; }

        [Column("skip_set_clock")]
        public int SkipClock { get; set; }

        [Column("skip_core_download")]
        public int SkipCore { get; set; }

        [Column("skip_volume_expand")]
        public int SkipExpandVolumes { get; set; }

        [Column("skip_lvm_shrink")]
        public int SkipShrinkLvm { get; set; }

        [Column("skip_volume_shrink")]
        public int SkipShrinkVolumes { get; set; }

        [Column("task_completed_action")]
        public string TaskCompletedAction { get; set; }

        [Column("upload_schema_only")]
        public int UploadSchemaOnly { get; set; }

        [Column("web_cancel")]
        public int WebCancel { get; set; }

        [Column("wim_enabled_multicast")]
        public int WimMulticastEnabled { get; set; }

        [Column("skip_nvram")]
        public int SkipNvramUpdate { get; set; }

        [Column("randomize_guids")]
        public int RandomizeGuids { get; set; }

        [Column("force_standard_efi")]
        public int ForceStandardEfi { get; set; }

        [Column("force_standard_legacy")]
        public int ForceStandardLegacy { get; set; }

        [Column("simple_upload_schema")]
        public int SimpleUploadSchema { get; set; }

        [Column("model_match")]
        public string ModelMatch { get; set; }

        [Column("model_match_type")]
        public string ModelMatchType { get; set; }

        [Column("skip_hibernation_check")]
        public int SkipHibernationCheck { get; set; }

        [Column("skip_bitlocker_check")]
        public int SkipBitlockerCheck { get; set; }
    }

    [NotMapped]
    public class ImageProfileWithImage : ImageProfileEntity
    {
        public ImageEntity Image { get; set; }
    }
}