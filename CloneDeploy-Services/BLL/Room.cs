using System.Collections.Generic;
using System.Linq;
using CloneDeploy_DataModel;
using CloneDeploy_Entities;
namespace CloneDeploy_App.BLL
{
    public static class Room
    {

        public static ActionResultEntity AddRoom(RoomEntity room)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateRoom(room, true);
                if (validationResult.Success)
                {
                    uow.RoomRepository.Insert(room);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new UnitOfWork())
            {
                return uow.RoomRepository.Count();
            }
        }

        public static bool DeleteRoom(int roomId)
        {
            using (var uow = new UnitOfWork())
            {
                uow.RoomRepository.Delete(roomId);
                uow.Save();
                return true;
            }
        }

        public static RoomEntity GetRoom(int roomId)
        {
            using (var uow = new UnitOfWork())
            {
                return uow.RoomRepository.GetById(roomId);
            }
        }

        public static List<RoomEntity> SearchRooms(string searchString = "")
        {
            using (var uow = new UnitOfWork())
            {
                return uow.RoomRepository.Get(searchString);
            }
        }

        public static ActionResultEntity UpdateRoom(RoomEntity room)
        {
            using (var uow = new UnitOfWork())
            {
                var validationResult = ValidateRoom(room, false);
                if (validationResult.Success)
                {
                    uow.RoomRepository.Update(room, room.Id);
                    uow.Save();
                    validationResult.Success = true;
                }

                return validationResult;
            }
        }

        public static ActionResultEntity ValidateRoom(RoomEntity room, bool isNewRoom)
        {
            var validationResult = new ActionResultEntity();

            if (string.IsNullOrEmpty(room.Name) || !room.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Room Name Is Not Valid";
                return validationResult;
            }

            if (isNewRoom)
            {
                using (var uow = new UnitOfWork())
                {
                    if (uow.RoomRepository.Exists(h => h.Name == room.Name))
                    {
                        validationResult.Success = false;
                        validationResult.Message = "This Room Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new UnitOfWork())
                {
                    var originalRoom = uow.RoomRepository.GetById(room.Id);
                    if (originalRoom.Name != room.Name)
                    {
                        if (uow.RoomRepository.Exists(h => h.Name == room.Name))
                        {
                            validationResult.Success = false;
                            validationResult.Message = "This Room Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
      
    }
}