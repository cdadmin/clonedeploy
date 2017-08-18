using System.Collections.Generic;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Common;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;

namespace CloneDeploy_App.Controllers
{
    public class NbiEntryController : ApiController
    {
        private readonly NbiEntryServices _nbiEntryServices;

        public NbiEntryController()
        {
            _nbiEntryServices = new NbiEntryServices();
        }

        [CustomAuth(Permission = AuthorizationStrings.UpdateAdmin)]
        public ActionResultDTO Post(List<NbiEntryEntity> entries)
        {
            return _nbiEntryServices.AddNbiEntries(entries);
        }
    }
}