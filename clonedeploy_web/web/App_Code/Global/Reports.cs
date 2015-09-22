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
using System.Data;
using System.Linq;

using Models;


namespace Global
{
    
    public class Reports
    {
        /*
        public DataTable LastMulticasts()
        {
            
            var table = new DataTable();
            table.Columns.Add("Group");
            table.Columns.Add("Date");
            using (var db = new DB())
            {
                var histories = (from h in db.History where h.Event == "Multicast" orderby h.EventDate descending select h).Take(5);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    var group = new Group();
                    group.Id = Convert.ToInt32(history.TypeId);
                    group.Read();
                    row["Group"] = group.Name;
                    row["Date"] = history.EventDate;
                    table.Rows.Add(row);

                }
              
            }
          
            return table;
        }

        public DataTable LastUnicasts()
        {
            var table = new DataTable();
            table.Columns.Add("Host");
            table.Columns.Add("Date");
            using (var db = new DB())
            {
                var histories = (from h in db.History where h.Event == "Deploy" || h.Event == "Upload" orderby h.EventDate descending select h).Take(5);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    var host = new BLL.Computer().GetComputer(Convert.ToInt16(history.TypeId));
                    row["Host"] = host.Name;
                    row["Date"] = history.EventDate;
                    table.Rows.Add(row);

                }

            }
          
            return table;
        }

        public DataTable LastUsers()
        {
            var table = new DataTable();
            table.Columns.Add("User");
            table.Columns.Add("Date");
            using (var db = new DB())
            {
                var histories = (from h in db.History where h.Event == "Successful Login" orderby h.EventDate descending select h).Take(5);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    var user = new WdsUser { Id = history.TypeId };
                    user.Read();
                    row["User"] = user.Name;
                    row["Date"] = history.EventDate;
                    table.Rows.Add(row);

                }

            }

            return table;
        }

        public DataTable TopFiveMulticast()
        {
            var table = new DataTable();
            table.Columns.Add("Group");
            table.Columns.Add("Count");
            using (var db = new DB())
            {
                var histories = (from h in db.History where h.Event == "Multicast" group h by h.TypeId into hgroup select new { groupCount = hgroup.Count(), hgroup.Key }).OrderByDescending(x => x.groupCount).Take(5);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    var group = new Group { Id = Convert.ToInt32(history.Key) };
                    group.Read();
                    row["Group"] = group.Name;
                    row["Count"] = history.groupCount;
                    table.Rows.Add(row);

                }

            }

            return table;
        }

        public DataTable TopFiveUnicast()
        {
            var table = new DataTable();
            table.Columns.Add("Host");
            table.Columns.Add("Count");
            using (var db = new DB())
            {
                var histories = (from h in db.History where h.Event == "Deploy" && h.Type == "Host" group h by h.TypeId into hgroup select new { groupCount = hgroup.Count(), hgroup.Key }).OrderByDescending(x => x.groupCount).Take(5);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    var host = new BLL.Computer().GetComputer(Convert.ToInt32(history.Key));
                    row["Host"] = host.Name;
                    row["Count"] = history.groupCount;
                    table.Rows.Add(row);

                }

            }

            return table;
        }

        public DataTable UserStats()
        {
            var table = new DataTable();
            table.Columns.Add("User");
            table.Columns.Add("Date");
            table.Columns.Add("Event");
            table.Columns.Add("Type");
            using (var db = new DB())
            {
                var histories = (from h in db.History orderby h.EventDate descending select h).Take(25);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    row["User"] = history.EventUser;
                    row["Date"] = history.EventDate;
                    row["Event"] = history.Event;
                    row["Type"] = history.Type;
                    table.Rows.Add(row);
                }

            }
            return table;
        }
         * */
    }
         
}