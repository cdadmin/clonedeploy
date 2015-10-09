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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("images")]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("image_id", Order = 1)]
        public int Id { get; set; }

        [Column("image_name", Order = 2)]
        public string Name { get; set; }

        [Column("image_os", Order = 3)]
        public string Os { get; set; }

        [Column("image_description", Order = 4)]
        public string Description { get; set; }

        [Column("image_is_protected", Order = 5)]
        public int Protected { get; set; }

        [Column("image_is_viewable_ond", Order = 6)]
        public int IsVisible { get; set; }

        [Column("image_checksum", Order = 7)]
        public string Checksum { get; set; }

        [Column("image_type", Order = 8)]
        public string Type { get; set; }

        [NotMapped]
        public string ClientSize { get; set; }

        [NotMapped]
        public string ClientSizeCustom { get; set; }


      
        
    }
}