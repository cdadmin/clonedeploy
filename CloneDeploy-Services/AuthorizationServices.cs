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

        public AuthorizationServices(int userId, string requiredRight)
        {
            _userServices = new UserServices();
            _cloneDeployUser = _userServices.GetUser(userId);
            _currentUserRights = _userServices.GetUserRights(userId).Select(right => right.Right).ToList();
            _requiredRight = requiredRight;
        }

        public bool ComputerManagement(int computerId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            if (_cloneDeployUser.GroupManagementEnabled == 1)
            {
                var computers = new ComputerServices().SearchComputersForUser(_cloneDeployUser.Id, int.MaxValue);
                return computers.Any(x => x.Id == computerId);
            }

            return IsAuthorized();
        }

        public bool GroupManagement(int groupId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            if (_cloneDeployUser.GroupManagementEnabled == 1)
            {
                return new GroupServices().SearchGroupsForUser(_cloneDeployUser.Id).Any(x => x.Id == groupId);
            }

            return IsAuthorized();
        }

        public bool ImageManagement(int imageId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            if (_cloneDeployUser.ImageManagementEnabled == 1)
            {
                return new ImageServices().SearchImagesForUser(_cloneDeployUser.Id).Any(x => x.Id == imageId);
            }
            return IsAuthorized();
        }

        public bool IsAuthorized()
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            if (_cloneDeployUser.Membership == "Service Account" && _requiredRight == "ServiceAccount") return true;

            return _currentUserRights.Any(right => right == _requiredRight);
        }
    }
}