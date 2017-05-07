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
    public class BuildingController : ApiController
    {
        private readonly BuildingServices _buildingServices;

        public BuildingController()
        {
            _buildingServices = new BuildingServices();
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _buildingServices.DeleteBuilding(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalRead")]
        public BuildingEntity Get(int id)
        {
            var result = _buildingServices.GetBuilding(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<BuildingWithClusterGroup> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _buildingServices.SearchBuildings()
                : _buildingServices.SearchBuildings(searchstring);
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _buildingServices.TotalCount()};
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(BuildingEntity building)
        {
            var result = _buildingServices.AddBuilding(building);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, BuildingEntity building)
        {
            building.Id = id;
            var result = _buildingServices.UpdateBuilding(building);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}