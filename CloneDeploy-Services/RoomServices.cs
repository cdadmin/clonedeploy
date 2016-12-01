using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
using CloneDeploy_Entities.DTOs;

namespace CloneDeploy_Services
{
    public class RoomServices
    {
        private readonly UnitOfWork _uow;

        public RoomServices()
        {
            _uow = new UnitOfWork();
        }

        public  ActionResultDTO AddRoom(RoomEntity room)
        {
           
                var validationResult = ValidateRoom(room, true);
            var actionResult = new ActionResultDTO();
                if (validationResult.Success)
                {
                    _uow.RoomRepository.Insert(room);
                    _uow.Save();
                    actionResult.Success = true;
                    actionResult.Id = room.Id;
                }
                else
                {
                    actionResult.ErrorMessage = validationResult.ErrorMessage;
                }

            return actionResult;

        }

        public  string TotalCount()
        {
           
                return _uow.RoomRepository.Count();
            
        }

        public ActionResultDTO DeleteRoom(int roomId)
        {
            var room = GetRoom(roomId);
            if (room == null) return new ActionResultDTO() {ErrorMessage = "Room Not Found", Id = 0};
            _uow.RoomRepository.Delete(roomId);
            _uow.Save();
            var actionResult = new ActionResultDTO();
            actionResult.Success = true;
            actionResult.Id = room.Id;
            return actionResult;

        }

        public  RoomEntity GetRoom(int roomId)
        {
           
                return _uow.RoomRepository.GetById(roomId);
            
        }

        public  List<RoomEntity> SearchRooms(string searchString = "")
        {
           
                return _uow.RoomRepository.Get(searchString);
            
        }

        public  ActionResultDTO UpdateRoom(RoomEntity room)
        {
            var r = GetRoom(room.Id);
            if (r == null) return new ActionResultDTO() { ErrorMessage = "Room Not Found", Id = 0 };
          
                var validationResult = ValidateRoom(room, false);
                var actionResult = new ActionResultDTO();
                if (validationResult.Success)
                {
                    _uow.RoomRepository.Update(room, room.Id);
                    _uow.Save();
                    
                    actionResult.Success = true;
                    actionResult.Id = room.Id;
                }

            return actionResult;

        }

        private ValidationResultDTO ValidateRoom(RoomEntity room, bool isNewRoom)
        {
            var validationResult = new ValidationResultDTO();

            if (string.IsNullOrEmpty(room.Name) || !room.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.ErrorMessage = "Room Name Is Not Valid";
                return validationResult;
            }

            if (isNewRoom)
            {
               
                    if (_uow.RoomRepository.Exists(h => h.Name == room.Name))
                    {
                        validationResult.Success = false;
                        validationResult.ErrorMessage = "This Room Already Exists";
                        return validationResult;
                    }
                
            }
            else
            {
              
                    var originalRoom = _uow.RoomRepository.GetById(room.Id);
                    if (originalRoom.Name != room.Name)
                    {
                        if (_uow.RoomRepository.Exists(h => h.Name == room.Name))
                        {
                            validationResult.Success = false;
                            validationResult.ErrorMessage = "This Room Already Exists";
                            return validationResult;
                        }
                    }
                
            }

            return validationResult;
        }
      
    }
}