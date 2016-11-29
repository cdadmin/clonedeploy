using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public class ImageProfileSysprepTag
    {

        public static bool AddImageProfileSysprepTag(ImageProfileSysprepTagEntity imageProfileSysprepTag)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileSysprepRepository.Insert(imageProfileSysprepTag);
                return uow.Save();
            }
        }

        public static bool DeleteImageProfileSysprepTags(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.ImageProfileSysprepRepository.DeleteRange(x => x.ProfileId == profileId);
                return uow.Save();
            }
        }

        public static List<ImageProfileSysprepTagEntity> SearchImageProfileSysprepTags(int profileId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.ImageProfileSysprepRepository.Get(x => x.ProfileId == profileId, orderBy: q => q.OrderBy(t => t.Priority));
            }
        }
    }
}