using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneDeploy_Entities
{
    public static class AuditEntry
    {
        public enum Type
        {

            Create = 1,
            Update = 2,
            Delete = 3,
            SuccessfulLogin = 4,
            FailedLogin = 5,
            Push = 6,
            Pull = 7,
            PermanentPush = 8,
            Multicast = 9,
        }
    }
}
