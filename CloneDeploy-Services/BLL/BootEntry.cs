using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using Newtonsoft.Json;

namespace CloneDeploy_App.BLL
{
    public class BootEntry
    {

        public static ActionResultEntity AddBootEntry(BootEntryEntity bootEntry)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateEntry(bootEntry, true);
                if (validationResult.Success)
                {
                    uow.BootEntryRepository.Insert(bootEntry);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = bootEntry.Id;
                    validationResult.Object = JsonConvert.SerializeObject(bootEntry);
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.BootEntryRepository.Count();
            }
        }

        public static ActionResultEntity DeleteBootEntry(int BootEntryId)
        {
            var actionResult = new ActionResultEntity();
            var bootEntry = GetBootEntry(BootEntryId);
            using (var uow = new UnitOfWork())
            {
                uow.BootEntryRepository.Delete(BootEntryId);
                actionResult.Success = uow.Save();
                actionResult.Object = JsonConvert.SerializeObject(bootEntry);
                actionResult.ObjectId = bootEntry.Id;
            }

            return actionResult;
        }

        public static BootEntryEntity GetBootEntry(int BootEntryId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.BootEntryRepository.GetById(BootEntryId);
            }
        }

        public static List<BootEntryEntity> SearchBootEntrys(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return
                    uow.BootEntryRepository.Get(
                        s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));
            }
        }

        public static ActionResultEntity UpdateBootEntry(BootEntryEntity bootEntry)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateEntry(bootEntry, false);
                if (validationResult.Success)
                {
                    uow.BootEntryRepository.Update(bootEntry, bootEntry.Id);
                    validationResult.Success = uow.Save();
                    validationResult.ObjectId = bootEntry.Id;
                    validationResult.Object = JsonConvert.SerializeObject(bootEntry);
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateEntry(BootEntryEntity bootEntry, bool isNewEntry)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(bootEntry.Name))
            {
                validationResult.Success = false;
                validationResult.Message = "Boot Entry Name Is Not Valid";
                return validationResult;
            }

            if (isNewEntry)
            {
                using (var uow = new UnitOfWork())
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
                using (var uow = new UnitOfWork())
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