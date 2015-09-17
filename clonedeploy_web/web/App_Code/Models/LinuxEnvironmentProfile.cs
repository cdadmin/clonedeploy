using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Global;

namespace Models
{
    [Table("linux_profiles", Schema = "public")]
    public class LinuxEnvironmentProfile
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
        public int SkipShrinkVolumes  { get; set; }

        [Column("skip_lvm_shrink", Order = 13)]
        public int SkipShrinkLvm { get; set; }

        [Column("resize_debug", Order = 14)]
        public int DebugResize { get; set; }

        [Column("skip_volume_expand", Order = 15)]
        public int SkipExpandVolumes { get; set; }

        [Column("fix_bcd", Order = 16)]
        public int FixBcd { get; set; }

        [Column("fix_bootloader", Order = 17)]
        public int FixBootloader { get; set; }

        [Column("partition_method", Order = 18)]
        public string PartitionMethod { get; set; }

        [Column("force_dynamic_partitions", Order = 19)]
        public int ForceDynamicPartitions { get; set; }

        [Column("always_expand_partitions", Order = 20)]
        public int ForceExpandPartitions { get; set; }

        [Column("custom_partition_script", Order = 21)]
        public string CustomPartitionScript { get; set; }


        public bool Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.ImageProfiles.Any(p => p.Name == Name))
                    {
                        Utility.Message = "This Profile Already Exists";
                        return false;
                    }
                    db.ImageProfiles.Add(this);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Profile.  Check The Exception Log For More Info.";
                    return false;
                }
            }

            GetProfileId();
            //var history = new History { Event = "Create", Type = "Host", Notes = Mac, TypeId = Id.ToString() };
            //history.CreateEvent();
            Utility.Message = "Successfully Created " + Name;
            return true;
        }

        private void GetProfileId()
        {
            using (var db = new DB())
            {
                var profile = db.ImageProfiles.First(p => p.Name == Name);
                Id = profile.Id;
            }
        }

        public string GetTotalCount()
        {
            using (var db = new DB())
            {
                return db.ImageProfiles.Count().ToString();
            }
        }

        public void Read()
        {
            using (var db = new DB())
            {
                var profile = db.ImageProfiles.FirstOrDefault(p => p.Id == Id);
                if (profile == null) return;
                this.Name = profile.Name;
                this.Description = profile.Description;
                this.Kernel = profile.Kernel;
                this.BootImage = profile.BootImage;
                this.KernelArguments = profile.KernelArguments;
                this.SkipCore = profile.SkipCore;
                this.SkipClock = profile.SkipClock;
                this.TaskCompletedAction = profile.TaskCompletedAction;
                this.RemoveGPT = profile.RemoveGPT;
                this.SkipShrinkVolumes = profile.SkipShrinkVolumes;
                this.SkipShrinkLvm = profile.SkipShrinkLvm;
                this.DebugResize = profile.DebugResize;
                this.SkipExpandVolumes = profile.SkipExpandVolumes;
                this.FixBcd = profile.FixBcd;
                this.FixBootloader = profile.FixBootloader;
                this.PartitionMethod = profile.PartitionMethod;
                this.ForceDynamicPartitions = profile.ForceDynamicPartitions;
                this.ForceExpandPartitions = profile.ForceExpandPartitions;
                this.CustomPartitionScript = profile.CustomPartitionScript;
            }
        }

        public List<LinuxEnvironmentProfile> Search(int imageId)
        {
            List<LinuxEnvironmentProfile> list = new List<LinuxEnvironmentProfile>();

                using (var db = new DB())
                {
                    list.AddRange(from p in db.ImageProfiles where p.ImageId == imageId orderby p.Name select p);
                }
            
            return list;
        }

        public void Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var profile = db.ImageProfiles.Find(this.Id);
                    if (profile != null)
                    {
                        profile.Name = this.Name;
                        profile.Description = this.Description;
                        profile.Kernel = this.Kernel;
                        profile.BootImage = this.BootImage;
                        profile.KernelArguments = this.KernelArguments;
                        profile.SkipCore = this.SkipCore;
                        profile.SkipClock = this.SkipClock;
                        profile.TaskCompletedAction = this.TaskCompletedAction;
                        profile.RemoveGPT = this.RemoveGPT;
                        profile.SkipShrinkVolumes = this.SkipShrinkVolumes;
                        profile.SkipShrinkLvm = this.SkipShrinkLvm;
                        profile.DebugResize = this.DebugResize;
                        profile.SkipExpandVolumes = this.SkipExpandVolumes;
                        profile.FixBcd = this.FixBcd;
                        profile.FixBootloader = this.FixBootloader;
                        profile.PartitionMethod = this.PartitionMethod;
                        profile.ForceDynamicPartitions = this.ForceDynamicPartitions;
                        profile.ForceExpandPartitions = this.ForceExpandPartitions;
                        profile.CustomPartitionScript = this.CustomPartitionScript;
                        db.SaveChanges();
                    }
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Image Profile.";
                    return;
                }
            }

            var history = new History
            {
                Event = "Edit",
                Type = "Host",
                TypeId = Id.ToString()
            };
            history.CreateEvent();
            Utility.Message = "Successfully Updated " + Name;
        }
    }
}