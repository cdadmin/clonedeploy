using System.Collections.Generic;
using System.Linq;

namespace CloneDeploy_App.BLL
{
    public class Partition
    {

        public static bool AddPartition(Models.Partition partition)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.PartitionRepository.Insert(partition);
                return uow.Save();
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.PartitionRepository.Count();
            }
        }

      
        public static bool DeletePartition(int partitionId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.PartitionRepository.Delete(partitionId);
                return uow.Save();
            }
        }

        public static Models.Partition GetPartition(int partitionId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.PartitionRepository.GetById(partitionId);
            }
        }

      
        public static List<Models.Partition> SearchPartitions(int layoutId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.PartitionRepository.Get(p => p.LayoutId == layoutId,
                    orderBy: (q => q.OrderBy(p => p.Number)));
            }
        }

        public static bool UpdatePartition(Models.Partition partition)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.PartitionRepository.Update(partition, partition.Id);
                return uow.Save();
            }

        }

     
    }
}