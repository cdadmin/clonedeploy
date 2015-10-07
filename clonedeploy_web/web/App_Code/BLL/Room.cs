using System.Collections.Generic;
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

        public bool AddRoom(Models.Room room)
        {
            if (_unitOfWork.RoomRepository.Exists(r => r.Name == room.Name))
            {
                Message.Text = "A Room With This Name Already Exists";
                return false;
            }
            _unitOfWork.RoomRepository.Insert(room);
            if (_unitOfWork.Save())
            {
                Message.Text = "Successfully Created Room";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Room";
                return false;
            }
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

        public void UpdateRoom(Models.Room room)
        {
            _unitOfWork.RoomRepository.Update(room, room.Id);
            if (_unitOfWork.Save())
                Message.Text = "Successfully Updated Room";
        }

      
    }
}