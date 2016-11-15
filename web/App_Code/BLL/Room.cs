using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public static class Room
    {
    
        public static Models.ActionResult AddRoom(Models.Room room)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateRoom(room, true);
                if (validationResult.Success)
                {
                    uow.RoomRepository.Insert(room);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.RoomRepository.Count();
            }
        }

        public static bool DeleteRoom(int roomId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.RoomRepository.Delete(roomId);
                return uow.Save();
            }
        }

        public static Models.Room GetRoom(int roomId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.RoomRepository.GetById(roomId);
            }
        }

        public static List<Models.Room> SearchRooms(string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.RoomRepository.Get(searchString);
            }
        }

        public static Models.ActionResult UpdateRoom(Models.Room room)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateRoom(room, false);
                if (validationResult.Success)
                {
                    uow.RoomRepository.Update(room, room.Id);
                    validationResult.Success = uow.Save();
                }

                return validationResult;
            }
        }

        public static Models.ActionResult ValidateRoom(Models.Room room, bool isNewRoom)
        {
            var validationResult = new Models.ActionResult();

            if (string.IsNullOrEmpty(room.Name) || !room.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.Success = false;
                validationResult.Message = "Room Name Is Not Valid";
                return validationResult;
            }

            if (isNewRoom)
            {
                using (var uow = new DAL.UnitOfWork())
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
                using (var uow = new DAL.UnitOfWork())
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