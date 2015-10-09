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

        public static Models.ValidationResult AddGroup(Models.Group group)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateGroup(group, true);
                if (validationResult.IsValid)
                {
                    uow.GroupRepository.Insert(group);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static string TotalCount()
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.Count();
            }
        }

      
        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupRepository.Delete(groupId);
                return uow.Save();
            }
        }

        public static Models.Group GetGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.GetById(groupId);
            }
        }

        public static List<Models.Group> SearchGroups(string searchString)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.Get(g => g.Name.Contains(searchString));
            }
        }

        public static Models.ValidationResult UpdateGroup(Models.Group group)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                var validationResult = ValidateGroup(group, false);
                if (validationResult.IsValid)
                {
                    uow.GroupRepository.Update(group, group.Id);
                    validationResult.IsValid = uow.Save();
                }

                return validationResult;
            }
        }

        public static void StartMulticast(Models.Group group)
        {
            var multicast = new Multicast { Group = group };
            multicast.Create();
        }

        public static void StartGroupUnicast(Models.Group group)
        {
            var image = BLL.Image.GetImage(group.Image);

            if (BLL.Image.Check_Checksum(image))
            {
                var count = 0;

                foreach (var host in GetGroupMembers(group.Id, ""))
                {
                    BLL.Computer.StartUnicast(host,"push");
               
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

        public static void ImportGroups()
        {
            throw new Exception("Not Implemented");
        }

       

        public static List<Models.Computer> GetGroupMembers(int groupId, string searchString = "")
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupRepository.GetGroupMembers(groupId, searchString);
            }
        }

        public static Models.ValidationResult ValidateGroup(Models.Group group, bool isNewGroup)
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