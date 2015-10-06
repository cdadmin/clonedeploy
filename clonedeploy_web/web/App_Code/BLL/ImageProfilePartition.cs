using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{
    public class ImageProfilePartition
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public ImageProfilePartition()
        {
            _unitOfWork = new DAL.UnitOfWork();
        }

        public bool AddImageProfilePartition(Models.ImageProfilePartition imageProfilePartition)
        {
            _unitOfWork.ImageProfilePartitionRepository.Insert(imageProfilePartition);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Added Partition Layout";
                return true;
            }
            else
            {
                Message.Text = "Could Not Add Partition Layout";
                return false;
            }
        }

        public bool DeleteImageProfilePartitions(int profileId)
        {
            _unitOfWork.ImageProfilePartitionRepository.DeleteRange(x => x.ProfileId == profileId);
            return _unitOfWork.Save();
        }

        public List<Models.ImageProfilePartition> SearchImageProfilePartitions(int profileId)
        {
           return _unitOfWork.ImageProfilePartitionRepository.Get(p => p.ProfileId == profileId,
                orderBy: (q => q.OrderBy(p => p.Id)));
        }
    }
}