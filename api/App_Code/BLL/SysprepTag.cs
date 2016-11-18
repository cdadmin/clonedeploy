﻿using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class SysprepTag
    {

        public static Models.ActionResult AddSysprepTag(Models.SysprepTag sysprepTag)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateSysprepTag(sysprepTag, true);
                if (validationResult.Success)
                {
                    uow.SysprepTagRepository.Insert(sysprepTag);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SysprepTagRepository.Count();
            }
        }

        public static bool DeleteSysprepTag(int sysprepTagId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.SysprepTagRepository.Delete(sysprepTagId);
                return uow.Save();
            }
        }

        public static Models.SysprepTag GetSysprepTag(int sysprepTagId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SysprepTagRepository.GetById(sysprepTagId);
            }
        }

        public static List<Models.SysprepTag> SearchSysprepTags(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.SysprepTagRepository.Get(
                        s => s.Name.Contains(searchString) || s.OpeningTag.Contains(searchString),
                        orderBy: (q => q.OrderBy(s => s.OpeningTag)));
            }
        }

        public static Models.ActionResult UpdateSysprepTag(Models.SysprepTag sysprepTag)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateSysprepTag(sysprepTag, false);
                if (validationResult.Success)
                {
                    uow.SysprepTagRepository.Update(sysprepTag, sysprepTag.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }



        }

        public static Models.ActionResult ValidateSysprepTag(Models.SysprepTag sysprepTag, bool isNewSysprepTag)
        {
            var validationResult = new Models.ActionResult();

            if (string.IsNullOrEmpty(sysprepTag.Name) || !sysprepTag.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Sysprep Tag Name Is Not Valid";
                return validationResult;
            }

            if (isNewSysprepTag)
            {
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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