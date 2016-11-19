using System.Collections.Generic;
using System.Linq;

namespace CloneDeploy_App.BLL
{
    public class ImageProfilePartition
    {

        public static bool AddImageProfilePartition(Models.ImageProfilePartitionLayout imageProfilePartition)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ImageProfilePartitionRepository.Insert(imageProfilePartition);
                return uow.Save();
            }
        }

        public static bool DeleteImageProfilePartitions(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.ImageProfilePartitionRepository.DeleteRange(x => x.ProfileId == profileId);
                return uow.Save();
            }
        }

        public static List<Models.ImageProfilePartitionLayout> SearchImageProfilePartitions(int profileId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.ImageProfilePartitionRepository.Get(p => p.ProfileId == profileId,
                    orderBy: (q => q.OrderBy(p => p.Id)));
            }
        }
    }
}