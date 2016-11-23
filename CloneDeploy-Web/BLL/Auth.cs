using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using CloneDeploy_Web.Models;
using Helpers;

namespace BLL
{
    public class Auth
    {
        
        private readonly CloneDeployUser _cloneDeployUser;
        private readonly List<string> _currentUserRights;
        private readonly string _requiredRight;

        public Auth(int userId, string requiredRight )
        {
            _cloneDeployUser = BLL.User.GetUser(userId);
            _currentUserRights = BLL.UserRight.Get(userId).Select(right => right.Right).ToList();
            _requiredRight = requiredRight;
        }

        public bool IsAuthorized()
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;
            return _currentUserRights.Any(right => right == _requiredRight);
        }

        public bool ComputerManagement(int computerId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            var userGroupManagements = BLL.UserGroupManagement.Get(_cloneDeployUser.Id);      
            if (userGroupManagements.Count > 0)
            {
                //Group management is in use since at least 1 result was returned.  Now check if allowed
                var computers = BLL.Computer.SearchComputersForUser(_cloneDeployUser.Id,Int32.MaxValue);
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

            var userGroupManagements = BLL.UserGroupManagement.Get(_cloneDeployUser.Id);
            if (userGroupManagements.Count > 0)
            {
                //Group management is in use since at least 1 result was returned.  Now check if allowed
                return BLL.Group.SearchGroupsForUser(_cloneDeployUser.Id).Any(x => x.Id == groupId);
            }
            else //Group management is not in use, use the global rights for the user
            {
                return IsAuthorized();
            }
        }

     
    }
}