using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;

namespace BLL
{
    public class Room
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public Room()
        {
            _unitOfWork = new UnitOfWork();
        }

        public Models.ValidationResult AddRoom(Models.Room room)
        {
            var validationResult = ValidateRoom(room, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.RoomRepository.Insert(room);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public string TotalCount()
        {
            return _unitOfWork.RoomRepository.Count();
        }

        public bool DeleteRoom(int roomId)
        {
            _unitOfWork.RoomRepository.Delete(roomId);
            return _unitOfWork.Save();
        }

        public Models.Room GetRoom(int roomId)
        {
            return _unitOfWork.RoomRepository.GetById(roomId);
        }

        public List<Models.Room> SearchRooms(string searchString)
        {
            return _unitOfWork.RoomRepository.Get(r => r.Name.Contains(searchString),includeProperties:"dp");
        }

        public Models.ValidationResult UpdateRoom(Models.Room room)
        {
            var validationResult = ValidateRoom(room, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.RoomRepository.Update(room, room.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
        }

        public Models.ValidationResult ValidateRoom(Models.Room room, bool isNewRoom)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(room.Name) || room.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Room Name Is Not Valid";
                return validationResult;
            }

            if (isNewRoom)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.RoomRepository.Exists(h => h.Name == room.Name))
                    {
                        validationResult.IsValid = false;
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
                            validationResult.IsValid = false;
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