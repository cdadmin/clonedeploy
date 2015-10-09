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

        public Models.ValidationResult AddGroup(Models.Group group)
        {
            var validationResult = ValidateGroup(group, true);
            if (validationResult.IsValid)
            {
                _unitOfWork.GroupRepository.Insert(group);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
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

        public Models.ValidationResult UpdateGroup(Models.Group group)
        {
            var validationResult = ValidateGroup(group, false);
            if (validationResult.IsValid)
            {
                _unitOfWork.GroupRepository.Update(group, group.Id);
                validationResult.IsValid = _unitOfWork.Save();
            }

            return validationResult;
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
                //Message.Text = "Started " + count + " Tasks";
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

       

        public List<Models.Computer> GetGroupMembers(int groupId, string searchString = "")
        {
            return _unitOfWork.GroupRepository.GetGroupMembers(groupId, searchString);
        }

        public Models.ValidationResult ValidateGroup(Models.Group group, bool isNewGroup)
        {
            var validationResult = new Models.ValidationResult();

            if (string.IsNullOrEmpty(group.Name) || group.Name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                validationResult.IsValid = false;
                validationResult.Message = "Group Name Is Not Valid";
                return validationResult;
            }

            if (isNewGroup)
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    if (uow.GroupRepository.Exists(h => h.Name == group.Name))
                    {
                        validationResult.IsValid = false;
                        validationResult.Message = "This Group Already Exists";
                        return validationResult;
                    }
                }
            }
            else
            {
                using (var uow = new DAL.UnitOfWork())
                {
                    var originalGroup = uow.GroupRepository.GetById(group.Id);
                    if (originalGroup.Name != group.Name)
                    {
                        if (uow.GroupRepository.Exists(h => h.Name == group.Name))
                        {
                            validationResult.IsValid = false;
                            validationResult.Message = "This Group Already Exists";
                            return validationResult;
                        }
                    }
                }
            }

            return validationResult;
        }
    }
}