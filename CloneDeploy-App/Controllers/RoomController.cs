using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_App.Controllers.Authorization;
using CloneDeploy_App.DTOs;
using CloneDeploy_App.Models;

namespace CloneDeploy_App.Controllers
{
    public class RoomController: ApiController
    {
        [GlobalAuth(Permission = "GlobalRead")]
        public IEnumerable<Models.Room> Get(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? BLL.Room.SearchRooms()
                : BLL.Room.SearchRooms(searchstring);

        }

        [GlobalAuth(Permission = "GlobalRead")]
        public ApiDTO GetCount()
        {
            var ApiDTO = new ApiDTO();
            ApiDTO.Value = BLL.Room.TotalCount();
            return ApiDTO;
        }

        [GlobalAuth(Permission = "GlobalRead")]
        public IHttpActionResult Get(int id)
        {
            var result = BLL.Room.GetRoom(id);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }

        [GlobalAuth(Permission = "GlobalCreate")]
        public ActionResult Post(Models.Room bootEntry)
        {
            var actionResult = BLL.Room.AddRoom(bootEntry);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalUpdate")]
        public Models.ActionResult Put(int id, Models.Room bootEntry)
        {
            bootEntry.Id = id;
            var actionResult = BLL.Room.UpdateRoom(bootEntry);
            if (!actionResult.Success)
            {
                var response = Request.CreateResponse(HttpStatusCode.NotFound, actionResult);
                throw new HttpResponseException(response);
            }
            return actionResult;
        }

        [GlobalAuth(Permission = "GlobalDelete")]
        public ApiBoolDTO Delete(int id)
        {
            var apiBoolDto = new ApiBoolDTO();
            apiBoolDto.Value = BLL.Room.DeleteRoom(id);
           return apiBoolDto;
        }
    }
}