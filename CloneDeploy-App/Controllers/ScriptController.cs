using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Services;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ScriptController: ApiController
    {
        private readonly ScriptServices _scriptServices;

        public ScriptController()
        {
            _scriptServices = new ScriptServices();
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<ScriptEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _scriptServices.SearchScripts()
                : _scriptServices.SearchScripts(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _scriptServices.TotalCount()};
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ScriptEntity Get(int id)
        {
            var result = _scriptServices.GetScript(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(ScriptEntity script)
        {
            return _scriptServices.AddScript(script);
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, ScriptEntity script)
        {
            script.Id = id;
            var result = _scriptServices.UpdateScript(script);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {

            var result = _scriptServices.DeleteScript(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}