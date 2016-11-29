using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class ImageProfileScript
    {

        public static bool AddImageProfileScript(ImageProfileScriptEntity imageProfileScript)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileScriptRepository.Insert(imageProfileScript);
                return uow.Save();
            }
        }

        public static bool DeleteImageProfileScripts(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileScriptRepository.DeleteRange(x => x.ProfileId == profileId);
                return uow.Save();
            }
        }

        public static List<ImageProfileScriptEntity> SearchImageProfileScripts(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageProfileScriptRepository.Get(x => x.ProfileId == profileId, orderBy: q => q.OrderBy(t => t.Priority));
            }
        }
    }
}