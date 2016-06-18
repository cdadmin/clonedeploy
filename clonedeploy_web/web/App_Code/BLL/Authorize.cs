using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Helpers;
using Models;

namespace BLL
{
    public class Authorize
    {
        private readonly CloneDeployUser _cloneDeployUser;
        private readonly List<string> _currentUserRights;
        private readonly string _requiredRight;

        public Authorize(CloneDeployUser user, string requiredRight )
        {
            _cloneDeployUser = user;
            _currentUserRights = BLL.UserRight.Get(_cloneDeployUser.Id).Select(right => right.Right).ToList();
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

        public bool ImageManagement(int imageId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            var userImageManagements = BLL.UserImageManagement.Get(_cloneDeployUser.Id);
            if (userImageManagements.Count > 0)
            {
                //Image management is in use since at least 1 result was returned.  Now check if allowed
                return BLL.Image.SearchImagesForUser(_cloneDeployUser.Id).Any(x => x.Id == imageId);
            }
            else //Image management is not in use, use the global rights for the user
            {
                return IsAuthorized();
            }
        }

        public static int ApiAuth()
        {
            if (!HttpContext.Current.Request.IsSecureConnection)
              ApiErrorResponse("ssl");

            var apiId = Utility.Decode(HttpContext.Current.Request.Headers["api-id"], "api-id");
            var apiKey = Utility.Decode(HttpContext.Current.Request.Headers["api-key"], "api-key");
          
            if(string.IsNullOrEmpty(apiId) || string.IsNullOrEmpty(apiKey))
                ApiErrorResponse("auth");

            var user = BLL.User.GetUserByApiId(apiId);
            if (user == null)
                ApiErrorResponse("auth");

            if (user.ApiKey != apiKey)             
                ApiErrorResponse("auth");

            return user.Id;
        }

        private static void ApiErrorResponse(string type)
        {
            var responseMsg = new HttpResponseMessage();
            if (type == "ssl")
            {
                responseMsg.StatusCode = HttpStatusCode.Forbidden;
                responseMsg.Content = new StringContent("SSL Required");
            }
            else
            {
                responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                responseMsg.Content = new StringContent("User not authorized");
            }

            throw new HttpResponseException(responseMsg);
        }
    }
}