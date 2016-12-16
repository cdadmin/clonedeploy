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
    public class ScriptController: ApiController
    {
        private readonly ScriptServices _scriptServices;

        public ScriptController()
        {
            _scriptServices = new ScriptServices();
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<ScriptEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _scriptServices.SearchScripts()
                : _scriptServices.SearchScripts(searchstring);

        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _scriptServices.TotalCount()};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public ScriptEntity Get(int id)
        {
            var result = _scriptServices.GetScript(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(ScriptEntity script)
        {
            return _scriptServices.AddScript(script);
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, ScriptEntity script)
        {
            script.Id = id;
            var result = _scriptServices.UpdateScript(script);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {

            var result = _scriptServices.DeleteScript(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}