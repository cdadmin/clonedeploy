using System;
using System.Collections.Generic;
using Global;

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

      
        public void DeleteGroup(int groupId)
        {
            _da.Delete(groupId);
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
                Utility.Message = "Group Name Cannot Be Empty Or Contain Spaces";
            }



            return validated;
        }
    }
}