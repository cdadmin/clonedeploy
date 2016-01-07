using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public static class ImageProfile
    {
        public static Models.ValidationResult AddProfile(Models.ImageProfile profile)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateLinuxProfile(profile, true);
                if (validationResult.IsValid)
                {
                    uow.LinuxProfileRepository.Insert(profile);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ImageProfile ReadProfile(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var imageProfile = uow.LinuxProfileRepository.GetById(profileId);
                if (imageProfile != null)
                    imageProfile.Image = BLL.Image.GetImage(imageProfile.ImageId);
                return imageProfile;
            }
        }

        public static bool DeleteProfile(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.LinuxProfileRepository.Delete(profileId);
                return uow.Save();
            }
        }

        public static List<Models.ImageProfile> SearchProfiles(int imageId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.LinuxProfileRepository.Get(p => p.ImageId == imageId,
                    orderBy: (q => q.OrderBy(p => p.Name)));
            }
        }

        public static Models.ValidationResult UpdateProfile(Models.ImageProfile profile)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateLinuxProfile(profile, false);
                if (validationResult.IsValid)
                {
                    uow.LinuxProfileRepository.Update(profile, profile.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }

        }

        public static Models.ValidationResult ValidateLinuxProfile(Models.ImageProfile linuxProfile, bool isNewLinuxProfile)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(linuxProfile.Name) || !linuxProfile.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Linux Profile Name Is Not Valid";
                return validationResult;
            }

            if (isNewLinuxProfile)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.LinuxProfileRepository.Exists(h => h.Name == linuxProfile.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Linux Profile Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalLinuxProfile = uow.LinuxProfileRepository.GetById(linuxProfile.Id);
                    if (originalLinuxProfile.Name != linuxProfile.Name)
                    {
                        if (uow.LinuxProfileRepository.Exists(h => h.Name == linuxProfile.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Linux Profile Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

        public static void SeedDefaultLinuxProfile(int imageId)
        {
            var imageProfile = new Models.ImageProfile();
            imageProfile.ImageId = imageId;
            imageProfile.Kernel = Settings.DefaultKernel32;
            imageProfile.BootImage = "initrd.xz";
            imageProfile.Name = "default";
            imageProfile.Description = "Auto Generated Via New Image.";
            imageProfile.SkipCore = 0;
            imageProfile.SkipClock = 0;
            imageProfile.RemoveGPT = 0;
            imageProfile.SkipShrinkVolumes = 0;
            imageProfile.SkipShrinkLvm = 0;
            imageProfile.SkipExpandVolumes = 0;
            imageProfile.FixBcd = 0;
            imageProfile.FixBootloader = 0;
            imageProfile.PartitionMethod = "Dynamic";
            imageProfile.Compression = "lz4";
            imageProfile.CompressionLevel = "1";
            imageProfile.TaskCompletedAction = "Reboot";
            AddProfile(imageProfile);
        }

        public static void CloneProfile(Models.ImageProfile imageProfile)
        {
            var originalName = imageProfile.Name;
            using (var uow = new DAL.UnitOfWork())
            {
                for (int c = 1; c <= 100; c++)
                {
                    var newProfileName = imageProfile.Name + "_" + c;
                    if (uow.LinuxProfileRepository.Exists(h => h.Name == newProfileName))
                        continue;

                    var clonedProfile = imageProfile;
                    clonedProfile.Name = newProfileName;
                    clonedProfile.Description = "Cloned From " + originalName;
                    AddProfile(clonedProfile);
                    break;
                }
            }
        }
    }
}