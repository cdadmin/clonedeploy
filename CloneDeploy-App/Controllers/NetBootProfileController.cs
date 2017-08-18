using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class NetBootProfileController : ApiController
    {
        private readonly NetBootProfileServices _netBootProfileServices;

        public NetBootProfileController()
        {
            _netBootProfileServices = new NetBootProfileServices();
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ActionResultDTO Delete(int id)
        {
            var result = _netBootProfileServices.DeleteProfile(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ApiBoolResponseDTO DeleteProfileNbiEntries(int id)
        {
            var result = _netBootProfileServices.DeleteProfileNbiEntries(id);
            return new ApiBoolResponseDTO {Value = result};
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadAdmin)]
        public NetBootProfileEntity Get(int id)
        {
            var result = _netBootProfileServices.GetProfile(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadAdmin)]
        public IEnumerable<NetBootProfileEntity> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _netBootProfileServices.SearchNetBootProfiles()
                : _netBootProfileServices.SearchNetBootProfiles(searchstring);
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadAdmin)]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO {Value = _netBootProfileServices.TotalCount()};
        }

        [CustomAuth(Permission = AuthorizationStrings.ReadAdmin)]
        public IEnumerable<NbiEntryEntity> GetProfileNbiEntries(int id)
        {
            return _netBootProfileServices.GetProfileNbiEntries(id);
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ActionResultDTO Post(NetBootProfileEntity profile)
        {
            return _netBootProfileServices.AddNetBootProfile(profile);
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ActionResultDTO Put(int id, NetBootProfileEntity profile)
        {
            profile.Id = id;
            var result = _netBootProfileServices.UpdateNetBootProfile(profile);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}