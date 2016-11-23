using System.Collections.Generic;
using CloneDeploy_Web.Models;

namespace BLL
{
    public static class Site
    {
        public static ActionResult AddSite(CloneDeploy_Web.Models.Site site)
        {
            using (var uow = new DAL.UnitOfWork())
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
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SiteRepository.Count();
            }
        }

        public static bool DeleteSite(int siteId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.SiteRepository.Delete(siteId);
                return uow.Save();
            }
        }

        public static CloneDeploy_Web.Models.Site GetSite(int siteId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SiteRepository.GetById(siteId);
            }
        }

        public static List<CloneDeploy_Web.Models.Site> SearchSites(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SiteRepository.Get(searchString);
            }
        }

        public static ActionResult UpdateSite(CloneDeploy_Web.Models.Site site)
        {
            using (var uow = new DAL.UnitOfWork())
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

        public static ActionResult ValidateSite(CloneDeploy_Web.Models.Site site, bool isNewSite)
        {
            var validationResult = new ActionResult();

            if (string.IsNullOrEmpty(site.Name))
            {
                validationResult.Success = false;
                validationResult.Message = "Site Name Is Not Valid";
                return validationResult;
            }

            if (isNewSite)
            {
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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