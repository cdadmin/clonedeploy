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
            table.Columns.Add("Computer");
            table.Columns.Add("Date");
            using (var db = new DB())
            {
                var histories = (from h in db.History where h.Event == "Deploy" || h.Event == "Upload" orderby h.EventDate descending select h).Take(5);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    var computer = new BLL.Computer().GetComputer(Convert.ToInt16(history.TypeId));
                    row["Computer"] = computer.Name;
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
            table.Columns.Add("Computer");
            table.Columns.Add("Count");
            using (var db = new DB())
            {
                var histories = (from h in db.History where h.Event == "Deploy" && h.Type == "Computer" group h by h.TypeId into hgroup select new { groupCount = hgroup.Count(), hgroup.Key }).OrderByDescending(x => x.groupCount).Take(5);
                foreach (var history in histories)
                {
                    var row = table.NewRow();
                    var computer = new BLL.Computer().GetComputer(Convert.ToInt32(history.Key));
                    row["Computer"] = computer.Name;
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