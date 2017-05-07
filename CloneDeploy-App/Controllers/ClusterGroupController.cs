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
    public class ClusterGroupController : ApiController
    {
        private readonly ClusterGroupServices _clusterGroupServices;

        public ClusterGroupController()
        {
            _clusterGroupServices = new ClusterGroupServices();
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Delete(int id)
        {
            var result = _clusterGroupServices.DeleteClusterGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "AdminRead")]
        public ClusterGroupEntity Get(int id)
        {
            var result = _clusterGroupServices.GetClusterGroup(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "AdminRead")]
        public IEnumerable<ClusterGroupEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _clusterGroupServices.SearchClusterGroups()
                : _clusterGroupServices.SearchClusterGroups(searchstring);
        }

        [CustomAuth(Permission = "AdminRead")]
        public IEnumerable<ClusterGroupDistributionPointEntity> GetClusterDistributionPoints(int id)
        {
            return _clusterGroupServices.GetClusterDps(id);
        }

        [CustomAuth(Permission = "AdminRead")]
        public IEnumerable<ClusterGroupServerEntity> GetClusterServers(int id)
        {
            return _clusterGroupServices.GetClusterServers(id);
        }

        [CustomAuth(Permission = "AdminRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _clusterGroupServices.TotalCount()};
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Post(ClusterGroupEntity clusterGroup)
        {
            return _clusterGroupServices.AddClusterGroup(clusterGroup);
        }

        [CustomAuth(Permission = "AdminUpdate")]
        public ActionResultDTO Put(int id, ClusterGroupEntity clusterGroup)
        {
            clusterGroup.Id = id;
            var result = _clusterGroupServices.UpdateClusterGroup(clusterGroup);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}