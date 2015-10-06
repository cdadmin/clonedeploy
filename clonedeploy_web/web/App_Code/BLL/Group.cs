using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using Helpers;
using Models;
using Tasks;

namespace BLL
{
    public class Group
    {
        private readonly DAL.UnitOfWork _unitOfWork;

        public Group()
        {
            _unitOfWork = new UnitOfWork();
        }

        public bool AddGroup(Models.Group group)
        {
            if (_unitOfWork.GroupRepository.Exists(g => g.Name == group.Name))
            {
                Message.Text = "A Group With This Name Already Exists";
                return false;
            }
            _unitOfWork.GroupRepository.Insert(group);
            if (_unitOfWork.Save())
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
            return _unitOfWork.GroupRepository.Count();
        }

      
        public bool DeleteGroup(int groupId)
        {
            _unitOfWork.GroupRepository.Delete(groupId);
            return _unitOfWork.Save();
        }

        public Models.Group GetGroup(int groupId)
        {
            return _unitOfWork.GroupRepository.GetById(groupId);
        }

        public List<Models.Group> SearchGroups(string searchString)
        {
            return _unitOfWork.GroupRepository.Get(g => g.Name.Contains(searchString));
        }

        public void UpdateGroup(Models.Group group)
        {
            _unitOfWork.GroupRepository.Update(group, group.Id);
            _unitOfWork.Save();
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

                foreach (var host in GetGroupMembers(group.Id, ""))
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

        public List<Models.Computer> GetGroupMembers(int groupId, string searchString)
        {
            return _unitOfWork.GroupRepository.GetGroupMembers(groupId, searchString);

        }
    }
}