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
            Deploy = 6,
            Upload = 7,
            PermanentPush = 8,
            Multicast = 9,
            OndMulticast = 10
        }
    }
}
