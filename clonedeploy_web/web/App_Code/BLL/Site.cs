using System.Collections.Generic;
using Helpers;

namespace BLL
{
    public class Site
    {
        public bool AddSite(Models.Site site)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                if (uow.SiteRepository.Exists(s => s.Name == site.Name))
                {
                    Message.Text = "A Site With This Name Already Exists";
                    return false;
                }
                uow.SiteRepository.Insert(site);
                if (uow.Save())
                {
                    Message.Text = "Successfully Created Site";
                    return true;
                }
                else
                {
                    Message.Text = "Could Not Create Site";
                    return false;
                }
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

        public void UpdateSite(Models.Site site)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.SiteRepository.Update(site, site.Id);
                if (uow.Save())
                    Message.Text = "Successfully Updated Site";
            }
        }

      
    }
}