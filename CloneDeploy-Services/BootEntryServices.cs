using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class BootEntryServices
    {
         private readonly UnitOfWork _uow;

        public BootEntryServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddBootEntry(BootEntryEntity bootEntry)
        {

            var validationResult = ValidateEntry(bootEntry, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.BootEntryRepository.Insert(bootEntry);
                _uow.Save();

                actionResult.Success = true;
                actionResult.Id = bootEntry.Id;

            }

            return actionResult;

        }

        public string TotalCount()
        {

            return _uow.BootEntryRepository.Count();

        }

        public ActionResultDTO DeleteBootEntry(int BootEntryId)
        {
            var actionResult = new ActionResultDTO();
            var bootEntry = GetBootEntry(BootEntryId);
            if (bootEntry == null) return new ActionResultDTO() {ErrorMessage = "Boot Entry Not Found", Id = 0};

            _uow.BootEntryRepository.Delete(BootEntryId);
            _uow.Save();
            actionResult.Success = true;

            actionResult.Id = bootEntry.Id;


            return actionResult;
        }

        public BootEntryEntity GetBootEntry(int BootEntryId)
        {

            return _uow.BootEntryRepository.GetById(BootEntryId);

        }

        public List<BootEntryEntity> SearchBootEntrys(string searchString = "")
        {

            return
                _uow.BootEntryRepository.Get(
                    s => s.Name.Contains(searchString), orderBy: (q => q.OrderBy(t => t.Name)));

        }

        public ActionResultDTO UpdateBootEntry(BootEntryEntity bootEntry)
        {

            var validationResult = ValidateEntry(bootEntry, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.BootEntryRepository.Update(bootEntry, bootEntry.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = bootEntry.Id;

            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;

        }

        private  ValidationResultDTO ValidateEntry(BootEntryEntity bootEntry, bool isNewEntry)
        {
            var validationResult = new ValidationResultDTO();

            if (string.IsNullOrEmpty(bootEntry.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Boot Entry Name Is Not Valid";
                return validationResult;
            }

            if (isNewEntry)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.BootEntryRepository.Exists(h => h.Name == bootEntry.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Boot Entry Already Exists";
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
                            validationResult.ErrorMessage = "This Boot Template Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

    }
}