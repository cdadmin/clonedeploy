using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class SiteController: ApiController
    {
        private readonly SiteServices _siteServices;

        public SiteController()
        {
            _siteServices = new SiteServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<SiteEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _siteServices.SearchSites()
                : _siteServices.SearchSites(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {

            return new ApiStringResponseDTO() {Value = _siteServices.TotalCount()};

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public SiteEntity Get(int id)
        {
            var result = _siteServices.GetSite(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(SiteEntity site)
        {
            return _siteServices.AddSite(site);
            
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, SiteEntity site)
        {
            site.Id = id;
            var result = _siteServices.UpdateSite(site);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _siteServices.DeleteSite(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}