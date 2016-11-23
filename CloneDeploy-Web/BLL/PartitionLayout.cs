using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class PartitionLayout
    {

        public static ActionResult AddPartitionLayout(CloneDeploy_Web.Models.PartitionLayout partitionLayout)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidatePartitionLayout(partitionLayout, true);
                if (validationResult.Success)
                {
                    uow.PartitionLayoutRepository.Insert(partitionLayout);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.PartitionLayoutRepository.Count();
            }
        }


        public static bool DeletePartitionLayout(int partitionLayoutId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.PartitionLayoutRepository.Delete(partitionLayoutId);
                return uow.Save();
            }
        }

        public static CloneDeploy_Web.Models.PartitionLayout GetPartitionLayout(int partitionLayoutId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.PartitionLayoutRepository.GetById(partitionLayoutId);
            }
        }


        public static List<CloneDeploy_Web.Models.PartitionLayout> SearchPartitionLayouts(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.PartitionLayoutRepository.Get(p => p.Name.Contains(searchString),
                    orderBy: (q => q.OrderBy(p => p.Name)));
            }
        }

        public static ActionResult UpdatePartitionLayout(CloneDeploy_Web.Models.PartitionLayout partitionLayout)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidatePartitionLayout(partitionLayout, false);
                if (validationResult.Success)
                {
                    uow.PartitionLayoutRepository.Update(partitionLayout, partitionLayout.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static ActionResult ValidatePartitionLayout(CloneDeploy_Web.Models.PartitionLayout partitionLayout, bool isNewPartitionLayout)
        {
            var validationResult = new ActionResult();

            if (string.IsNullOrEmpty(partitionLayout.Name) || partitionLayout.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Partition Layout Name Is Not Valid";
                return validationResult;
            }

            if (isNewPartitionLayout)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.PartitionLayoutRepository.Exists(h => h.Name == partitionLayout.Name))
                    {
                        validationResult.Success = false;
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
                            validationResult.Success = false;
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