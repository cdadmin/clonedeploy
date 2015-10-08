using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class Partition
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public Partition()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool AddPartition(Models.Partition partition)
        {
            _unitOfWork.PartitionRepository.Insert(partition);
            return _unitOfWork.Save();
        }

        public string TotalCount()
        {
            return _unitOfWork.PartitionRepository.Count();
        }

      
        public bool DeletePartition(int partitionId)
        {
            _unitOfWork.PartitionRepository.Delete(partitionId);
            return _unitOfWork.Save();
        }

        public Models.Partition GetPartition(int partitionId)
        {
            return _unitOfWork.PartitionRepository.GetById(partitionId);
        }

      
        public List<Models.Partition> SearchPartitions(int layoutId)
        {
            return _unitOfWork.PartitionRepository.Get(p => p.LayoutId == layoutId,
                orderBy: (q => q.OrderBy(p => p.Number)));
        }

        public bool UpdatePartition(Models.Partition partition)
        {
            _unitOfWork.PartitionRepository.Update(partition,partition.Id);
            return _unitOfWork.Save();
              
        }

     
    }
}