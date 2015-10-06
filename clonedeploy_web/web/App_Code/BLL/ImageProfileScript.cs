using System.Collections.Generic;
using DAL;
using Helpers;

namespace BLL
{
    public class ImageProfileScript
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public ImageProfileScript()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool AddImageProfileScript(Models.ImageProfileScript imageProfileScript)
        {
            _unitOfWork.ImageProfileScriptRepository.Insert(imageProfileScript);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Added Script";
                return true;
            }
            else
            {
                Message.Text = "Could Not Add Script";
                return false;
            }
        }

        public bool DeleteImageProfileScripts(int profileId)
        {
            _unitOfWork.ImageProfileScriptRepository.DeleteRange(x => x.ProfileId == profileId);
            return _unitOfWork.Save();
        }

        public List<Models.ImageProfileScript> SearchImageProfileScripts(int profileId)
        {
            return _unitOfWork.ImageProfileScriptRepository.Find(profileId);
        }
    }
}