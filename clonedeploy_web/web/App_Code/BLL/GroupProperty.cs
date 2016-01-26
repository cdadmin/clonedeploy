using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;

namespace BLL
{
    public static class GroupProperty
    {
        public static void AddGroupProperty(Models.GroupProperty groupProperty)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                    uow.GroupPropertyRepository.Insert(groupProperty);
                    uow.Save();  
            }

            UpdateComputerProperties(groupProperty);
        }

        public static Models.GroupProperty GetGroupProperty(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                return uow.GroupPropertyRepository.GetFirstOrDefault(x => x.GroupId == groupId);
            }
        }

        public static void UpdateGroupProperty(Models.GroupProperty groupProperty)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                  uow.GroupPropertyRepository.Update(groupProperty, groupProperty.Id);
                  uow.Save();
            }
            UpdateComputerProperties(groupProperty);
        }

        public static void UpdateComputerProperties(Models.GroupProperty groupProperty)
        {
            foreach (var computer in BLL.Group.GetGroupMembers(groupProperty.GroupId))
            {
                if (Convert.ToBoolean(groupProperty.ImageEnabled))
                    computer.ImageId = groupProperty.ImageId;
                if (Convert.ToBoolean(groupProperty.ImageProfileEnabled))
                    computer.ImageProfileId = groupProperty.ImageProfileId;
                if (Convert.ToBoolean(groupProperty.DescriptionEnabled))
                    computer.Description = groupProperty.Description;
                if (Convert.ToBoolean(groupProperty.SiteEnabled))
                    computer.SiteId = groupProperty.SiteId;
                if (Convert.ToBoolean(groupProperty.BuildingEnabled))
                    computer.BuildingId = groupProperty.BuildingId;
                if (Convert.ToBoolean(groupProperty.RoomEnabled))
                    computer.RoomId = groupProperty.RoomId;
                if (Convert.ToBoolean(groupProperty.CustomAttribute1Enabled))
                    computer.CustomAttribute1 = groupProperty.CustomAttribute1;
                if (Convert.ToBoolean(groupProperty.CustomAttribute2Enabled))
                    computer.CustomAttribute2 = groupProperty.CustomAttribute2;
                if (Convert.ToBoolean(groupProperty.CustomAttribute3Enabled))
                    computer.CustomAttribute3 = groupProperty.CustomAttribute3;
                if (Convert.ToBoolean(groupProperty.CustomAttribute4Enabled))
                    computer.CustomAttribute4 = groupProperty.CustomAttribute4;
                if (Convert.ToBoolean(groupProperty.CustomAttribute5Enabled))
                    computer.CustomAttribute5 = groupProperty.CustomAttribute5;
                BLL.Computer.UpdateComputer(computer);
            }
        }

        public static bool DeleteGroup(int groupId)
        {
            using (var uow = new DAL.UnitOfWork())
            {
                uow.GroupPropertyRepository.DeleteRange(x => x.GroupId == groupId);
                return uow.Save();
            }
        }
       

      
    }
}