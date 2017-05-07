using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloneDeploy_Entities;

namespace CloneDeploy_DataModel
{
    public class CloneDeployUserRepository : GenericRepository<CloneDeployUserEntity>
    {
        private CloneDeployDbContext _context;

        public CloneDeployUserRepository(CloneDeployDbContext context)
            : base(context)
        {
            _context = context;
        }

        public List<UserWithUserGroup> Search(string searchString)
        {
            return (from s in _context.Users
                    join d in _context.UserGroups on s.UserGroupId equals d.Id into joined
                    from j in joined.DefaultIfEmpty()
                    where s.Name.Contains(searchString)
                    orderby s.Name
                    select new
                    {
                        id = s.Id,
                        name = s.Name,
                        membership = s.Membership,
                        notifyComplete = s.NotifyComplete,
                        notifyError = s.NotifyError,
                        notifyApproved = s.NotifyImageApproved,
                        notifyLockout = s.NotifyLockout,
                        userGroup = j
                       
                    }).AsEnumerable().Select(x => new UserWithUserGroup()
                    {
                        Id = x.id,
                        Name = x.name,
                        Membership = x.membership,
                        NotifyComplete = x.notifyComplete,
                        NotifyError = x.notifyError,
                        NotifyImageApproved = x.notifyApproved,
                        NotifyLockout = x.notifyLockout,
                        UserGroup = x.userGroup
                       
                    }).ToList();

        }
    
    }
}
