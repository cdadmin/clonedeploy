using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Helpers;

/// <summary>
/// Summary description for ParameterReplace
/// </summary>
public class ParameterReplace
{

    public static string Between(string parameter)
    {
        int start = parameter.IndexOf("[", StringComparison.Ordinal);
        int to = parameter.IndexOf("]", start + "[".Length, StringComparison.Ordinal);
        if (start < 0 || to < 0) return parameter;
        string s = parameter.Substring(
                       start + "[".Length,
                       to - start - "[".Length);
        if (s == "server-ip")
        {
            return Settings.ServerIp;
        }
        return s;
    }
}