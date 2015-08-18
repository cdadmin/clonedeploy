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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Global;

namespace Models
{
    [Table("boottemplates", Schema = "public")]
    public class BootTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("templateid", Order = 1)]
        public int Id { get; set; }

        [Column("templatecontent", Order = 2)]
        public string Content { get; set; }

        [Column("templatename", Order = 3)]
        public string Name { get; set; }

        public void Create()
        {
            using (var db = new DB())
            {
                try
                {
                    if (db.BootTemplates.Any(b => b.Name == Name))
                    {
                        Utility.Message = "This Template already exists";
                        return;
                    }
                    db.BootTemplates.Add(this);
                    db.SaveChanges();

                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Create Template.  Check The Exception Log For More Info.";
                }

                Utility.Message = "Successfully Created Template";
            }
        }

        public void Delete()
        {
            using (var db = new DB())
            {
                try
                {
                    db.BootTemplates.Attach(this);
                    db.BootTemplates.Remove(this);
                    db.SaveChanges();
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Delete Template.  Check The Exception Log For More Info.";
                    return;
                }

                Utility.Message = "Successfully Deleted Template";
          }
        }

        public void Read()
        {
            using (var db = new DB())
            {
                var template = db.BootTemplates.First(b => b.Name == Name);
                Name = template.Name;
                Content = template.Content;
            }
        }

        public void Update()
        {
            using (var db = new DB())
            {
                try
                {
                    var oldTemplate = db.BootTemplates.Attach(this);
                    oldTemplate.Name = Name;
                    oldTemplate.Content = Content;
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Utility.Message = "Could Not Update Template.  Check The Exception Log For More Info.";
                }
            }
        }

        public List<string> ListAll()
        {
            List<string> list = new List<string>();
            using (var db = new DB())
            {
                list.AddRange(from b in db.BootTemplates orderby b.Name select b.Name);
            }
            return list;
        }
    }
}