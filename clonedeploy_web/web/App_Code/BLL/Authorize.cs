using System.Collections.Generic;
using System.Linq;
using Models;

namespace BLL
{

    public class Authorize
    {
        private readonly WdsUser _cloneDeployUser;
        private readonly List<string> _currentUserRights;
        private readonly string _requiredRight;

        public Authorize(WdsUser user, string requiredRight )
        {
            _cloneDeployUser = user;
            _currentUserRights = BLL.UserRight.Get(_cloneDeployUser.Id).Select(right => right.Right).ToList();
            _requiredRight = requiredRight;
        }

        public bool Check()
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;
            return _currentUserRights.Any(right => right == _requiredRight);
        }

        public bool GroupManagement(int computerId)
        {
            if (_cloneDeployUser.Membership == "Administrator") return true;

            //All user rights don't have the required right.  No need to check group membership.
            if (_currentUserRights.All(right => right != _requiredRight)) return false;

            var userGroupManagements = BLL.UserGroupManagement.Get(_cloneDeployUser.Id);      
            if (userGroupManagements.Count > 0)
            {
                //Group management is in use since at least 1 result was returned.  Now check if allowed
                var computers = BLL.Computer.SearchComputersForUser("", _cloneDeployUser.Id);
                return computers.Any(x => x.Id == computerId);
            }

            return false;
        }
    }
}