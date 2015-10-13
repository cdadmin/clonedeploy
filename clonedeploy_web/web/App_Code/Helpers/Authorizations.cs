namespace Helpers
{
    public class Authorizations
    {
        public static string Administrator
        {
            get { return "Administrator"; }
        }

        public static string CreateComputer
        {
            get { return "ComputerCreate"; }
        }

        public static string ReadComputer
        {
            get { return "ComputerRead"; }
        }

        public static string UpdateComputer
        {
            get { return "ComputerUpdate"; }
        }

        public static string DeleteComputer
        {
            get { return "ComputerDelete"; }
        }
    }
}