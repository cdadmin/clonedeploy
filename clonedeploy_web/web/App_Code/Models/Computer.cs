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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.IdentityModel.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Global;

namespace Models
{
    [Table("computers", Schema = "public")]
    public class Computer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("computer_id", Order = 1)]
        public int Id { get; set; }

        [Column("computer_name", Order = 2)]
        public string Name { get; set; }

        [Column("computer_primary_mac", Order = 3)]
        public string Mac { get; set; }

        [Column("computer_description", Order = 4)]
        public string Description { get; set; }

        [Column("computer_building_id", Order = 5)]
        public int Building { get; set; }

        [Column("computer_room_id", Order = 6)]
        public int Room { get; set; }

        [Column("computer_image_id", Order = 7)]
        public int Image { get; set; }

        [Column("computer_image_profile_id", Order = 8)]
        public int ImageProfile { get; set; }
   
        [NotMapped]
        public string CustomBootEnabled { get; set; }

       
        [NotMapped]
        public string TaskId { get; set; }

        [NotMapped]
        public string ImageName { get; set; }

    }
}