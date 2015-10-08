using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class LinuxProfile
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public LinuxProfile()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Models.ValidationResult AddProfile(Models.LinuxProfile profile)
        {
            var validationResult = ValidateLinuxProfile(profile, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.LinuxProfileRepository.Insert(profile);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public Models.LinuxProfile ReadProfile(int profileId)
        {
            return _unitOfWork.LinuxProfileRepository.GetById(profileId);
        }

        public List<Models.LinuxProfile> SearchProfiles(int? imageId)
        {
            return _unitOfWork.LinuxProfileRepository.Get(p => p.ImageId == imageId,
                orderBy: (q => q.OrderBy(p => p.Name)));
        }

        public Models.ValidationResult UpdateProfile(Models.LinuxProfile profile)
        {
            var validationResult = ValidateLinuxProfile(profile, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.LinuxProfileRepository.Update(profile, profile.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
          
        }

        public Models.ValidationResult ValidateLinuxProfile(Models.LinuxProfile linuxProfile, bool isNewLinuxProfile)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(linuxProfile.Name) || linuxProfile.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
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