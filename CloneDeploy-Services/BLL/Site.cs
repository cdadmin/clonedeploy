using System.Collections.Generic;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class Site
    {
        public static ActionResultEntity AddSite(SiteEntity site)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateSite(site, true);
                if (validationResult.Success)
                {
                    uow.SiteRepository.Insert(site);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.SiteRepository.Count();
            }
        }

        public static bool DeleteSite(int siteId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.SiteRepository.Delete(siteId);
                return uow.Save();
            }
        }

        public static SiteEntity GetSite(int siteId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.SiteRepository.GetById(siteId);
            }
        }

        public static List<SiteEntity> SearchSites(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.SiteRepository.Get(searchString);
            }
        }

        public static ActionResultEntity UpdateSite(SiteEntity site)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateSite(site, false);
                if (validationResult.Success)
                {
                    uow.SiteRepository.Update(site, site.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateSite(SiteEntity site, bool isNewSite)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(site.Name))
            {
                validationResult.Success = false;
                validationResult.Message = "Site Name Is Not Valid";
                return validationResult;
            }

            if (isNewSite)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.SiteRepository.Exists(h => h.Name == site.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Site Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalSite = uow.SiteRepository.GetById(site.Id);
                    if (originalSite.Name != site.Name)
                    {
                        if (uow.SiteRepository.Exists(h => h.Name == site.Name))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This Site Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }

      
    }
}