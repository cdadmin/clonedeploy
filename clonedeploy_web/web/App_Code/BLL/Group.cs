using System;
using System.Collections.Generic;
using Helpers;
using Models;
using Tasks;

namespace BLL
{
    /// <summary>
    /// Summary description for Group
    /// </summary>
    public class Group
    {
        private readonly DAL.Group _da = new DAL.Group();

        public bool AddGroup(Models.Group group)
        {
            if (_da.Exists(group))
            {
                Message.Text = "A Group With This Name Already Exists";
                return false;
            }
            if (_da.Create(group))
            {
                Message.Text = "Successfully Created Group";
                return true;
            }
            else
            {
                Message.Text = "Could Not Create Group";
                return false;
            }
        }

        public string TotalCount()
        {
            return _da.GetTotalCount();
        }

      
        public bool DeleteGroup(int groupId)
        {
            return _da.Delete(groupId);
        }

        public Models.Group GetGroup(int groupId)
        {
            return _da.Read(groupId);
        }

        public List<Models.Group> SearchGroups(string searchString)
        {
            return _da.Find(searchString);
        }

        public void UpdateGroup(Models.Group group)
        {
            _da.Update(group);
        }

        public void StartMulticast(Models.Group group)
        {
            var multicast = new Multicast { Group = group };
            multicast.Create();
        }

        public void StartGroupUnicast(Models.Group group)
        {
            var bllImage = new Image();
            var image = bllImage.GetImage(group.Image);

            if (bllImage.Check_Checksum(image))
            {
                var count = 0;

                foreach (var host in new GroupMembership().GetGroupMembers(group.Id, ""))
                {
                    new Computer().StartUnicast(host,"push");
               
                    count++;
                }
                Message.Text = "Started " + count + " Tasks";
                var history = new History
                {
                    Event = "Unicast",
                    Type = "Group",
                    TypeId = group.Id.ToString()
                };
                history.CreateEvent();
            }
        }

        public void ImportGroups()
        {
            throw new Exception("Not Implemented");
        }

        public bool ValidateGroupData(Models.Group group)
        {
            var validated = true;
            if (string.IsNullOrEmpty(group.Name) || group.Name.Contains(" "))
            {
                validated = false;
                Message.Text = "Group Name Cannot Be Empty Or Contain Spaces";
            }



            return validated;
        }
    }
}