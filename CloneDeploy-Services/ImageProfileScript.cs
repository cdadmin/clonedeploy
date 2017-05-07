using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class ImageProfileScriptServices
    {
        private readonly UnitOfWork _uow;

        public ImageProfileScriptServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddImageProfileScript(ImageProfileScriptEntity imageProfileScript)
        {
            _uow.ImageProfileScriptRepository.Insert(imageProfileScript);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = imageProfileScript.Id;
            return actionResult;
        }
    }
}