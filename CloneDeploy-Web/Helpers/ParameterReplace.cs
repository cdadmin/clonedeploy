using System;

namespace Helpers
{
    /// <summary>
    /// Summary description for ParameterReplace
    /// </summary>
    public class ParameterReplace
    {

        public static string Between(string parameter)
        {
            if(string.IsNullOrEmpty(parameter)) return parameter;
            int start = parameter.IndexOf("[", StringComparison.Ordinal);
            int to = parameter.IndexOf("]", start + "[".Length, StringComparison.Ordinal);
            if (start < 0 || to < 0) return parameter;
            string s = parameter.Substring(
                start + "[".Length,
                to - start - "[".Length);
            if (s == "server-ip")
            {
                return parameter.Replace("[server-ip]", Settings.ServerIp);
            }
            return s;
        }
    }
}