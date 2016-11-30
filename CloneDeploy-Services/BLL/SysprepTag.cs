using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class SysprepTag
    {
        //moved
        public static ActionResultEntity AddSysprepTag(SysprepTagEntity sysprepTag)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateSysprepTag(sysprepTag, true);
                if (validationResult.Success)
                {
                    uow.SysprepTagRepository.Insert(sysprepTag);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }
        }

        //moved
        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.SysprepTagRepository.Count();
            }
        }

        //moved
        public static bool DeleteSysprepTag(int sysprepTagId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.SysprepTagRepository.Delete(sysprepTagId);
                uow.Save();
                return true;
            }
        }

        //moved
        public static SysprepTagEntity GetSysprepTag(int sysprepTagId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.SysprepTagRepository.GetById(sysprepTagId);
            }
        }

        //moved
        public static List<SysprepTagEntity> SearchSysprepTags(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return
                    uow.SysprepTagRepository.Get(
                        s => s.Name.Contains(searchString) || s.OpeningTag.Contains(searchString),
                        orderBy: (q => q.OrderBy(s => s.OpeningTag)));
            }
        }

        //moved
        public static ActionResultEntity UpdateSysprepTag(SysprepTagEntity sysprepTag)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateSysprepTag(sysprepTag, false);
                if (validationResult.Success)
                {
                    uow.SysprepTagRepository.Update(sysprepTag, sysprepTag.Id);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }



        }

        //move not needed
        public static ActionResultEntity ValidateSysprepTag(SysprepTagEntity sysprepTag, bool isNewSysprepTag)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(sysprepTag.Name) || !sysprepTag.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Sysprep Tag Name Is Not Valid";
                return validationResult;
            }

            if (isNewSysprepTag)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.SysprepTagRepository.Exists(h => h.Name == sysprepTag.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Sysprep Tag Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalSysprepTag = uow.SysprepTagRepository.GetById(sysprepTag.Id);
                    if (originalSysprepTag.Name != sysprepTag.Name)
                    {
                        if (uow.SysprepTagRepository.Exists(h => h.Name == sysprepTag.Name))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This Sysprep Tag Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

      
    }
}