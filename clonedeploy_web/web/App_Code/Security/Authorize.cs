using System.Web;
using Models;

namespace Security
{
    /// <summary>
    ///     Summary description for Authorize
    /// </summary>
    public class Authorize
    {
        public bool IsInMembership(string membership)
        {
            WdsUser user = new WdsUser {Name = HttpContext.Current.User.Identity.Name};
            user.Read();
            return membership == user.Membership;
        }
    }
}