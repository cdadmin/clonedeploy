using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{
    public class Site
    {
        public Models.ValidationResult AddSite(Models.Site site)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateSite(site, true);
                if (validationResult.IsValid)
                {
                    uow.SiteRepository.Insert(site);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SiteRepository.Count();
            }
        }

        public bool DeleteSite(int siteId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.SiteRepository.Delete(siteId);
                return uow.Save();
            }
        }

        public Models.Site GetSite(int siteId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SiteRepository.GetById(siteId);
            }
        }

        public List<Models.Site> SearchSites(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.SiteRepository.Get(s => s.Name.Contains(searchString), includeProperties: "dp");
            }
        }

        public Models.ValidationResult UpdateSite(Models.Site site)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateSite(site, false);
                if (validationResult.IsValid)
                {
                    uow.SiteRepository.Update(site, site.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public Models.ValidationResult ValidateSite(Models.Site site, bool isNewSite)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(site.Name) || site.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Site Name Is Not Valid";
                return validationResult;
            }

            if (isNewSite)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.SiteRepository.Exists(h => h.Name == site.Name))
                    {
                        validationResult.IsValid = false;
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
                            validationResult.IsValid = false;
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