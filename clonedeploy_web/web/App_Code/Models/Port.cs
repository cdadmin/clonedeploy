using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Global;

namespace Models
{
    [Table("ports", Schema = "public")]
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("portid", Order = 1)]
        public string Id { get; set; }

        [Column("portnumber", Order = 2)]
        public int Number { get; set; }

        public void Create()
        {
            using (var db = new DB())
            {
                db.Ports.Add(this);
                db.SaveChanges();
            }
        }

        public void GetNextPort()
        {
            using (var db = new DB())
            {
                var port = db.Ports.OrderByDescending(p => p.Id).FirstOrDefault();
                if (port == null)
                    Number = Convert.ToInt16(Settings.StartPort);
                else if (port.Number >= Convert.ToInt16(Settings.EndPort))
                    Number = Convert.ToInt16(Settings.StartPort);
                else
                    Number = port.Number + 2;

                Create();
            }
        }

        public string GetPort()
        {
            var port = new Port();
            port.GetNextPort();
            return port.Number.ToString();
        }
    }
}