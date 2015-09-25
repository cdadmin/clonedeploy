using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using DAL;
using Global;

namespace Models
{
    [Table("history", Schema = "public")]
    public class History
    {
        public History()
        {
            EventDate = DateTime.Now.ToString("MM-dd-yy h:mm:ss tt");
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("historyid", Order = 1)]
        public string Id { get; set; }

        [Column("historytime", Order = 2)]
        public string EventDate { get; set; }

        [Column("historytype", Order = 3)]
        public string Type { get; set; }

        [Column("historyevent", Order = 4)]
        public string Event { get; set; }

        [Column("historyip", Order = 5)]
        public string Ip { get; set; }

        [Column("historyuser", Order = 6)]
        public string EventUser { get; set; }
       
        [Column("historynotes", Order = 7)]     
        public string Notes { get; set; }
        
        [Column("historytypeid", Order = 8)]
        public string TypeId { get; set; }

        [NotMapped]
        public string Limit { get; set; }


        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public void CreateEvent()
        {
            //FIXME
            /*
            try
            {



                object objHistoryIp = Ip;
                if (objHistoryIp == null)
                    Ip = (string) HttpContext.Current.Session["ip_address"];
            }
            catch
            {
            }
        
            using (var db = new DB())
            {
                try
                {
                    db.History.Add(this);
                    //db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                }
            }
             * */
        }



        public List<History> Read()
        {
            if (Limit == "All")
                Limit = "9999";
            List<History> list = new List<History>();

                list.AddRange((from h in _context.History
                    where h.Type == Type && h.TypeId == TypeId
                    orderby h.EventDate descending
                    select h).Take(Convert.ToInt16(Limit)));              

            return list;
        }

        public List<History> ReadUser()
        {
            if (Limit == "All")
                Limit = "9999";
            List<History> list = new List<History>();

                list.AddRange((from h in _context.History
                               where (h.Type == Type && h.TypeId == TypeId) || h.EventUser == EventUser
                               orderby h.EventDate descending
                               select h).Take(Convert.ToInt16(Limit)));
            
            return list;
        }
    }
}