/*  
    CrucibleWDS A Windows Deployment Solution
    Copyright (C) 2011  Jon Dolny

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Global
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
            var text = new List<string>();
            var logPath = HttpContext.Current.Server.MapPath("~") + Path.DirectorySeparatorChar + "data" +
                          Path.DirectorySeparatorChar + "logs" + Path.DirectorySeparatorChar;

            try
            {
                text = File.ReadLines(logPath + log).Reverse().Take(Convert.ToInt16(limit)).Reverse().ToList();
            }
            catch (Exception ex)
            {
                Utility.Message = ex.Message.Replace("\\", "\\\\");
            }

            return text;
        }
    }
}