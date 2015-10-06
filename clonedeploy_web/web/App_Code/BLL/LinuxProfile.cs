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

        public bool AddProfile(Models.LinuxProfile profile)
        {
            if (_unitOfWork.LinuxProfileRepository.Exists(p => p.Name == profile.Name))
            {
                Message.Text = "A Profile With This Name Already Exists";
                return false;
            }
            _unitOfWork.LinuxProfileRepository.Insert(profile);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Profile";
                return true;
            }
            Message.Text = "Could Not Create Profile";
            return false;
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

        public void UpdateProfile(Models.LinuxProfile profile)
        {
            _unitOfWork.LinuxProfileRepository.Update(profile, profile.Id);
        }
    }
}