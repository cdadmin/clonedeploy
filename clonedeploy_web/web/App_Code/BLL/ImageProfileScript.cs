using System.Collections.Generic;
using System.Linq;
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
            return _unitOfWork.Save();          
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