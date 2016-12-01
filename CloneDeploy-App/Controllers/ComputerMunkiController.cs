using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.BLL;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;
using CloneDeploy_Services;


namespace CloneDeploy_App.Controllers
{
    public class ComputerMunkiController:ApiController
    {

        private readonly ComputerMunkiServices _computerMunkiServices;

        public ComputerMunkiController()
        {
            _computerMunkiServices = new ComputerMunkiServices();
        }

        [ComputerAuth(Permission = "GlobalRead")]
        public IEnumerable<ComputerMunkiEntity> GetTemplateComputers(int id)
        {
            var result = _computerMunkiServices.GetComputersForManifestTemplate(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [ComputerAuth(Permission = "ComputerCreate")]
        public ActionResultDTO Post(ComputerMunkiEntity computerMunki)
        {
            var result = _computerMunkiServices.AddMunkiTemplates(computerMunki);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

       
    }
}