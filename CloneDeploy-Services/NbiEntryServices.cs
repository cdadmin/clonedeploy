using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class NbiEntryServices
    {
        private readonly UnitOfWork _uow;

        public NbiEntryServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddNbiEntries(List<NbiEntryEntity> nbiEntries)
        {
            if (nbiEntries.Count == 0) return new ActionResultDTO() { ErrorMessage = "NBI List Was Empty", Id = 0, Success = false };
            foreach (var nbiEntry in nbiEntries)
            {
                var validationResult = ValidateEntry(nbiEntry);
                if (!validationResult.Success)
                {
                    return new ActionResultDTO() {ErrorMessage = validationResult.ErrorMessage, Id = 0, Success = false};
                }
            }

            foreach (var nbiEntry in nbiEntries)
            {
                _uow.NbiEntryRepository.Insert(nbiEntry);
            }

            _uow.Save();
            return new ActionResultDTO() {Success = true};
        }

       

        private ValidationResultDTO ValidateEntry(NbiEntryEntity entry)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(entry.NbiName) || entry.NbiName.Any(c => c == ' '))
            {
                validationResult.ErrorMessage = "Nbi Name Is Not Valid";
                validationResult.Success = false;
                return validationResult;
            }

            if (entry.NbiId < 1 || entry.NbiId > 65535)
            {
                validationResult.ErrorMessage = "Nbi Id Is Not Valid";
                validationResult.Success = false;
                return validationResult;
            }

            return validationResult;
        }
    }
}