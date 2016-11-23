using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ImageProfileScript
    {

        public static bool AddImageProfileScript(CloneDeploy_Web.Models.ImageProfileScript imageProfileScript)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ImageProfileScriptRepository.Insert(imageProfileScript);
                return uow.Save();
            }
        }

        public static bool DeleteImageProfileScripts(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ImageProfileScriptRepository.DeleteRange(x => x.ProfileId == profileId);
                return uow.Save();
            }
        }

        public static List<CloneDeploy_Web.Models.ImageProfileScript> SearchImageProfileScripts(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ImageProfileScriptRepository.Get(x => x.ProfileId == profileId, orderBy: q => q.OrderBy(t => t.Priority));
            }
        }
    }
}