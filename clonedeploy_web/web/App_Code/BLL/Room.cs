using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;

namespace BLL
{
    public class Room
    {
        private readonly DAL.Room _da = new DAL.Room();

        public bool AddRoom(Models.Room room)
        {
            if (_da.Exists(room.Name))
            {
                Message.Text = "A Room With This Name Already Exists";
                return false;
            }
            if (_da.Create(room))
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
            return _da.GetTotalCount();
        }

        public bool DeleteRoom(int roomId)
        {
            return _da.Delete(roomId);
        }

        public Models.Room GetRoom(int roomId)
        {
            return _da.Read(roomId);
        }

        public List<Models.Room> SearchRooms(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateRoom(Models.Room room)
        {
            if (_da.Update(room))
                Message.Text = "Successfully Updated Room";
        }

      
    }
}