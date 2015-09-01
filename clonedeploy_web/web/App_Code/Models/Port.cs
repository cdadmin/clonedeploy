using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Global;

namespace Models
{
    [Table("multicast_ports", Schema = "public")]
    public class Port
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("multicast_port_id", Order = 1)]
        public int Id { get; set; }

        [Column("multicast_port_number", Order = 2)]
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

        public int GetPort()
        {
            var port = new Port();
            port.GetNextPort();
            return port.Number;
        }
    }
}