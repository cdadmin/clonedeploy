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

        public Models.ValidationResult AddPartitionLayout(Models.PartitionLayout partitionLayout)
        {
            var validationResult = ValidatePartitionLayout(partitionLayout, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.PartitionLayoutRepository.Insert(partitionLayout);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
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

        public Models.ValidationResult UpdatePartitionLayout(Models.PartitionLayout partitionLayout)
        {
            var validationResult = ValidatePartitionLayout(partitionLayout, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.PartitionLayoutRepository.Update(partitionLayout, partitionLayout.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;         
        }

        public Models.ValidationResult ValidatePartitionLayout(Models.PartitionLayout partitionLayout, bool isNewPartitionLayout)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(partitionLayout.Name) || partitionLayout.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Partition Layout Name Is Not Valid";
                return validationResult;
            }

            if (isNewPartitionLayout)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.PartitionLayoutRepository.Exists(h => h.Name == partitionLayout.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Partition Layout Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalPartitionLayout = uow.PartitionLayoutRepository.GetById(partitionLayout.Id);
                    if (originalPartitionLayout.Name != partitionLayout.Name)
                    {
                        if (uow.PartitionLayoutRepository.Exists(h => h.Name == partitionLayout.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Partition Layout Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
    }
}