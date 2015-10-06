using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class PartitionLayout
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public PartitionLayout()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool AddPartitionLayout(Models.PartitionLayout partitionLayout)
        {
            _unitOfWork.PartitionLayoutRepository.Insert(partitionLayout);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Partition Layout";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Partition Layout";
                return false;
            }
        }

        public string TotalCount()
        {
            return _unitOfWork.PartitionLayoutRepository.Count();
        }


        public bool DeletePartitionLayout(int partitionLayoutId)
        {
            _unitOfWork.PartitionLayoutRepository.Delete(partitionLayoutId);
            return _unitOfWork.Save();
        }

        public Models.PartitionLayout GetPartitionLayout(int partitionLayoutId)
        {
            return _unitOfWork.PartitionLayoutRepository.GetById(partitionLayoutId);
        }


        public List<Models.PartitionLayout> SearchPartitionLayouts(string searchString)
        {
            return _unitOfWork.PartitionLayoutRepository.Get(p => p.Name.Contains(searchString),
                orderBy: (q => q.OrderBy(p => p.Name)));
        }

        public void UpdatePartitionLayout(Models.PartitionLayout partitionLayout)
        {
            _unitOfWork.PartitionLayoutRepository.Update(partitionLayout,partitionLayout.Id);
            if (_unitOfWork.Save())
                Message.Text = "Successfully Updated Partition Layout";
        }
    }
}