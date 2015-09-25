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
using DAL;
using Global;
using Helpers;

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

        private readonly CloneDeployDbContext _context = new CloneDeployDbContext();

        public void Create()
        {
           
                try
                {
                    if (_context.BootTemplates.Any(b => b.Name == Name))
                    {
                        Message.Text = "This Template already exists";
                        return;
                    }
                    _context.BootTemplates.Add(this);
                    _context.SaveChanges();

                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Message.Text = "Could Not Create Template.  Check The Exception Log For More Info.";
                }

                Message.Text = "Successfully Created Template";
            
        }

        public void Delete()
        {
          
                try
                {
                    _context.BootTemplates.Attach(this);
                    _context.BootTemplates.Remove(this);
                    _context.SaveChanges();
                }

                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Message.Text = "Could Not Delete Template.  Check The Exception Log For More Info.";
                    return;
                }

                Message.Text = "Successfully Deleted Template";
          
        }

        public void Read()
        {
           
                var template = _context.BootTemplates.First(b => b.Name == Name);
                Name = template.Name;
                Content = template.Content;
            
        }

        public void Update()
        {
          
                try
                {
                    var oldTemplate = _context.BootTemplates.Attach(this);
                    oldTemplate.Name = Name;
                    oldTemplate.Content = Content;
                    _context.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Logger.Log(ex.InnerException.InnerException.Message);
                    Message.Text = "Could Not Update Template.  Check The Exception Log For More Info.";
                }
            
        }

        public List<string> ListAll()
        {
            List<string> list = new List<string>();
          
                list.AddRange(from b in _context.BootTemplates orderby b.Name select b.Name);
            
            return list;
        }
    }
}