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
    public class RoomController: ApiController
    {
        private readonly RoomServices _roomServices;

        public RoomController()
        {
            _roomServices = new RoomServices();    
        }

        [CustomAuth(Permission = "GlobalRead")]
        public IEnumerable<RoomEntity> GetAll(string searchstring = "")
        {
            return string.IsNullOrEmpty(searchstring)
                ? _roomServices.SearchRooms()
                : _roomServices.SearchRooms(searchstring);

        }

        [CustomAuth(Permission = "GlobalRead")]
        public ApiStringResponseDTO GetCount()
        {
            return new ApiStringResponseDTO() {Value = _roomServices.TotalCount()};
        }

        [CustomAuth(Permission = "GlobalRead")]
        public RoomEntity Get(int id)
        {
            var result = _roomServices.GetRoom(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalCreate")]
        public ActionResultDTO Post(RoomEntity room)
        {
            return _roomServices.AddRoom(room);            
        }

        [CustomAuth(Permission = "GlobalUpdate")]
        public ActionResultDTO Put(int id, RoomEntity room)
        {
            room.Id = id;
            var result = _roomServices.UpdateRoom(room);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }

        [CustomAuth(Permission = "GlobalDelete")]
        public ActionResultDTO Delete(int id)
        {
            var result = _roomServices.DeleteRoom(id);
            if (result == null) throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            return result;
        }
    }
}