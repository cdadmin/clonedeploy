using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class SiteServices
    {
        private readonly UnitOfWork _uow;

        public SiteServices()
        {
            _uow = new UnitOfWork();
        }

        public ActionResultDTO AddSite(SiteEntity site)
        {
            var validationResult = ValidateSite(site, true);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.SiteRepository.Insert(site);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = site.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        public ActionResultDTO DeleteSite(int siteId)
        {
            var site = GetSite(siteId);
            if (site == null) return new ActionResultDTO {ErrorMessage = "Site Not Found", Id = 0};
            _uow.SiteRepository.Delete(siteId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = site.Id;
            return actionResult;
        }

        public SiteEntity GetSite(int siteId)
        {
            return _uow.SiteRepository.GetById(siteId);
        }

        public List<SiteWithClusterGroup> SearchSites(string searchString = "")
        {
            return _uow.SiteRepository.Get(searchString);
        }

        public string TotalCount()
        {
            return _uow.SiteRepository.Count();
        }

        public ActionResultDTO UpdateSite(SiteEntity site)
        {
            var s = GetSite(site.Id);
            if (s == null) return new ActionResultDTO {ErrorMessage = "Site Not Found", Id = 0};
            var validationResult = ValidateSite(site, false);
            var actionResult = new ActionResultDTO();
            if (validationResult.Success)
            {
                _uow.SiteRepository.Update(site, site.Id);
                _uow.Save();
                actionResult.Success = true;
                actionResult.Id = site.Id;
            }
            else
            {
                actionResult.ErrorMessage = validationResult.ErrorMessage;
            }

            return actionResult;
        }

        private ValidationResultDTO ValidateSite(SiteEntity site, bool isNewSite)
        {
            var validationResult = new ValidationResultDTO {Success = true};

            if (string.IsNullOrEmpty(site.Name))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Site Name Is Not Valid";
                return validationResult;
            }

            if (isNewSite)
            {
                if (_uow.SiteRepository.Exists(h => h.Name == site.Name))
                {
                    validationResult.Success = false;
                    validationResult.ErrorMessage = "This Site Already Exists";
                    return validationResult;
                }
            }
            else
            {
                var originalSite = _uow.SiteRepository.GetById(site.Id);
                if (originalSite.Name != site.Name)
                {
                    if (_uow.SiteRepository.Exists(h => h.Name == site.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Site Already Exists";
                        return validationResult;
                    }
                }
            }

            return validationResult;
        }
    }
}