using System.Collections.Generic;
using System.Linq;
using CloneDeploy_App.Helpers;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class ImageProfile
    {
        public static ActionResultEntity AddProfile(ImageProfileEntity profile)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateImageProfile(profile, true);
                if (validationResult.Success)
                {
                    uow.ImageProfileRepository.Insert(profile);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static ImageProfileEntity ReadProfile(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                var imageProfile = uow.ImageProfileRepository.GetById(profileId);
                if (imageProfile != null)
                    imageProfile.Image = BLL.Image.GetImage(imageProfile.ImageId);
                return imageProfile;
            }
        }

        public static bool DeleteProfile(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileRepository.Delete(profileId);
                return uow.Save();
            }
        }

        public static List<ImageProfileEntity> SearchProfiles(int imageId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageProfileRepository.Get(p => p.ImageId == imageId,
                    orderBy: (q => q.OrderBy(p => p.Name)));
            }
        }

        public static List<ImageProfileEntity> GetAllProfiles()
        {
            List<ImageProfileEntity> imageProfiles;
            using (var uow = new UnitOfWork())
            {
                imageProfiles = uow.ImageProfileRepository.Get(orderBy: (q => q.OrderBy(p => p.Name)));
            }

            foreach (var imageProfile in imageProfiles)
            {
                imageProfile.Image = BLL.Image.GetImage(imageProfile.ImageId);
            }

            return imageProfiles.OrderBy(x => x.Image.Name).ToList();
            
        }

        public static ActionResultEntity UpdateProfile(ImageProfileEntity profile)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateImageProfile(profile, false);
                if (validationResult.Success)
                {
                    uow.ImageProfileRepository.Update(profile, profile.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }

        }

        public static bool DeleteImage(int imageId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileRepository.DeleteRange(x => x.ImageId == imageId);
                return uow.Save();
            }
        }

        public static ActionResultEntity ValidateImageProfile(ImageProfileEntity imageProfile, bool isNewImageProfile)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(imageProfile.Name) || !imageProfile.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Image Profile Name Is Not Valid";
                return validationResult;
            }

            if (isNewImageProfile)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.ImageProfileRepository.Exists(h => h.Name == imageProfile.Name && h.ImageId == imageProfile.ImageId))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Image Profile Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalImageProfile = uow.ImageProfileRepository.GetById(imageProfile.Id);
                    if (originalImageProfile.Name != imageProfile.Name)
                    {
                        if (uow.ImageProfileRepository.Exists(h => h.Name == imageProfile.Name && h.ImageId == imageProfile.ImageId))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This Image Profile Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

        public static ImageProfileEntity SeedDefaultImageProfile(ImageEntity image)
        {
            var imageProfile = new ImageProfileEntity();
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
            imageProfile.FixBootloader = 1;
            imageProfile.PartitionMethod = "Dynamic";
            imageProfile.Compression = "lz4";
            imageProfile.CompressionLevel = "1";
            imageProfile.TaskCompletedAction = "Reboot";
            imageProfile.ChangeName = 1;
            if (image.Environment == "macOS")
                imageProfile.OsxTargetVolume = "Macintosh HD";

            return imageProfile;
        }

        public static void CloneProfile(ImageProfileEntity imageProfile)
        {
            var originalName = imageProfile.Name;
            using (var uow = new UnitOfWork())
            {
                for (int c = 1; c <= 100; c++)
                {
                    var newProfileName = imageProfile.Name + "_" + c;
                    if (uow.ImageProfileRepository.Exists(h => h.Name == newProfileName))
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