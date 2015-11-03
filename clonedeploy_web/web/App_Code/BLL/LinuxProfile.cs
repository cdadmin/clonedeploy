using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public static class LinuxProfile
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
                imageProfile.Image = BLL.Image.GetImage(imageProfile.ImageId);
                return imageProfile;
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
    }
}