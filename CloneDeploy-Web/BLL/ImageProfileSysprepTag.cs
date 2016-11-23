using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class ImageProfileSysprepTag
    {

        public static bool AddImageProfileSysprepTag(CloneDeploy_Web.Models.ImageProfileSysprepTag imageProfileSysprepTag)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ImageProfileSysprepRepository.Insert(imageProfileSysprepTag);
                return uow.Save();
            }
        }

        public static bool DeleteImageProfileSysprepTags(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ImageProfileSysprepRepository.DeleteRange(x => x.ProfileId == profileId);
                return uow.Save();
            }
        }

        public static List<CloneDeploy_Web.Models.ImageProfileSysprepTag> SearchImageProfileSysprepTags(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ImageProfileSysprepRepository.Get(x => x.ProfileId == profileId, orderBy: q => q.OrderBy(t => t.Priority));
            }
        }
    }
}