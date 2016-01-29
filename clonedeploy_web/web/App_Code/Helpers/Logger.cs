using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Helpers
{
    public class Logger
    {
        public static void Log(string message)
        {
            try
            {
                var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                             Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + "exceptions.log";

                File.AppendAllText(logPath,
                    DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ":\t" + message +
                    Environment.NewLine);
            }
            catch
            {
                // ignored
            }
        }

        public static List<string> ViewLog(string log, string limit)
        {
            if (limit == "All")
                limit = "9999";
         
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            return File.ReadLines(logPath + log).Reverse().Take(Convert.ToInt16(limit)).Reverse().ToList();

        }
    }
}