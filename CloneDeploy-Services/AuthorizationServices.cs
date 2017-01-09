using System;
using System.Collections.Generic;
using System.Linq;
using CloneDeploy_Entities;

namespace CloneDeploy_Services
{
    public class AuthorizationServices
    {

        private readonly CloneDeployUserEntity _cloneDeployUser;
        private readonly List<string> _currentUserRights;
        private readonly string _requiredRight;
        private readonly UserServices _userServices;



        public AuthorizationServices(int userId, string requiredRight )
        {
            _userServices = new UserServices();
            _cloneDeployUser = _userServices.GetUser(userId);
            _currentUserRights = _userServices.GetUserRights(userId).Select(right => right.Right).ToList();
            _requiredRight = requiredRight;
        }

        public bool IsAuthorized()
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;
            
            if (_cloneDeployUser.Membership == "Service Account" && _requiredRight == "ServiceAccount") return true;
            
            return _currentUserRights.Any(right => right == _requiredRight);
        }

        public bool ComputerManagement(int computerId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            var userGroupManagements = _userServices.GetUserGroupManagements(_cloneDeployUser.Id);      
            if (userGroupManagements.Count > 0)
            {
                //Group management is in use since at least 1 result was returned.  Now check if allowed
                var computers = new ComputerServices().SearchComputersForUser(_cloneDeployUser.Id,Int32.MaxValue);
                return computers.Any(x => x.Id == computerId);
            }
            else //Group management is not in use, use the global rights for the user
            {
                return IsAuthorized();
            }
        }

        public bool GroupManagement(int groupId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            var userGroupManagements = _userServices.GetUserGroupManagements(_cloneDeployUser.Id);
            if (userGroupManagements.Count > 0)
            {
                //Group management is in use since at least 1 result was returned.  Now check if allowed
                return new GroupServices().SearchGroupsForUser(_cloneDeployUser.Id).Any(x => x.Id == groupId);
            }
            else //Group management is not in use, use the global rights for the user
            {
                return IsAuthorized();
            }
        }

        public bool ImageManagement(int imageId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            var userImageManagements = _userServices.GetUserImageManagements(_cloneDeployUser.Id);
            if (userImageManagements.Count > 0)
            {
                //Image management is in use since at least 1 result was returned.  Now check if allowed
                return new ImageServices().SearchImagesForUser(_cloneDeployUser.Id).Any(x => x.Id == imageId);
            }
            else //Image management is not in use, use the global rights for the user
            {
                return IsAuthorized();
            }
        }

     
    }
}