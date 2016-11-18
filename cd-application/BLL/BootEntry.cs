using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class BootEntry
    {

        public static Models.ActionResult AddBootEntry(Models.BootEntry bootEntry)
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

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BootEntryRepository.Count();
            }
        }

        public static bool DeleteBootEntry(int BootEntryId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.BootEntryRepository.Delete(BootEntryId);
                return uow.Save();
            }
        }

        public static Models.BootEntry GetBootEntry(int BootEntryId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.BootEntryRepository.GetById(BootEntryId);
            }
        }

        public static List<Models.BootEntry> SearchBootEntrys(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return
                    uow.BootEntryRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static Models.ActionResult UpdateBootEntry(Models.BootEntry bootEntry)
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

        public static Models.ActionResult ValidateEntry(Models.BootEntry bootEntry, bool isNewEntry)
        {
            var validationResult = new Models.ActionResult();

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