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

namespace Models
{
    [Table("groups", Schema = "public")]
    public class Group
    {
        public Group()
        {
            Members = new List<Computer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("group_id", Order = 1)]
        public int Id { get; set; }

        [Column("group_name", Order = 2)]
        public string Name { get; set; }

        [Column("group_description", Order = 3)]
        public string Description { get; set; }

        [Column("group_image_id", Order = 4)]
        public int Image { get; set; }

        [Column("group_image_profile_id", Order = 5)]
        public int ImageProfile { get; set; }

        [Column("group_type", Order = 6)]
        public string Type { get; set; }

        [Column("group_sender_arguments", Order = 7)]
        public string SenderArguments { get; set; }

        [Column("group_receiver_arguments", Order = 8)]
        public string ReceiverArguments { get; set; }

        [NotMapped] 
        public List<Computer> Members { get; set; }

       
    }
}