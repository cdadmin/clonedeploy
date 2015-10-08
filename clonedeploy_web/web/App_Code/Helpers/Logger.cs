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
                //var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                  //            Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar + "exceptions.log";
                var logPath = @"c:\mount\exceptions.log";
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
            var text = new List<string>();
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            try
            {
                text = File.ReadLines(logPath + log).Reverse().Take(Convert.ToInt16(limit)).Reverse().ToList();
            }
            catch (Exception ex)
            {
                //Message.Text = ex.Message.Replace("\\", "\\\\");
            }

            return text;
        }
    }
}