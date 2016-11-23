using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Web.Models;

namespace BLL
{
    public class BootEntry
    {

        //moved
        public static ActionResult AddBootEntry(CloneDeploy_Web.Models.BootEntry bootEntry)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateEntry(bootEntry, true);
                if (validationResult.Success)
                {
                    uow.BootEntryRepository.Insert(bootEntry);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        //moved
        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BootEntryRepository.Count();
            }
        }

        //moved
        public static bool DeleteBootEntry(int BootEntryId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.BootEntryRepository.Delete(BootEntryId);
                return uow.Save();
            }
        }

        //moved
        public static CloneDeploy_Web.Models.BootEntry GetBootEntry(int BootEntryId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BootEntryRepository.GetById(BootEntryId);
            }
        }

        //moved
        public static List<CloneDeploy_Web.Models.BootEntry> SearchBootEntrys(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.BootEntryRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        //moved
        public static ActionResult UpdateBootEntry(CloneDeploy_Web.Models.BootEntry bootEntry)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateEntry(bootEntry, false);
                if (validationResult.Success)
                {
                    uow.BootEntryRepository.Update(bootEntry, bootEntry.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        //move not needed
        public static ActionResult ValidateEntry(CloneDeploy_Web.Models.BootEntry bootEntry, bool isNewEntry)
        {
            var validationResult = new ActionResult();

            if (string.IsNullOrEmpty(bootEntry.Name))
            {
                validationResult.Success = false;
                validationResult.Message = "Boot Entry Name Is Not Valid";
                return validationResult;
            }

            if (isNewEntry)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.BootEntryRepository.Exists(h => h.Name == bootEntry.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Boot Entry Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalTemplate = uow.BootEntryRepository.GetById(bootEntry.Id);
                    if (originalTemplate.Name != bootEntry.Name)
                    {
                        if (uow.BootEntryRepository.Exists(h => h.Name == bootEntry.Name))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This Boot Template Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

    }
}