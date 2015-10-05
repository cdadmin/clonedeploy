using System.Collections.Generic;
using Helpers;

namespace BLL
{
    public class Site
    {
        private readonly DAL.Site _da = new DAL.Site();

        public bool AddSite(Models.Site site)
        {
            if (_da.Exists(site.Name))
            {
                Message.Text = "A Site With This Name Already Exists";
                return false;
            }
            if (_da.Create(site))
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

        public string TotalCount()
        {
            return _da.GetTotalCount();
        }

        public bool DeleteSite(int siteId)
        {
            return _da.Delete(siteId);
        }

        public Models.Site GetSite(int siteId)
        {
            return _da.Read(siteId);
        }

        public List<Models.Site> SearchSites(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateSite(Models.Site site)
        {
            if (_da.Update(site))
                Message.Text = "Successfully Updated Site";
        }

      
    }
}