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
    [Table("clonedeploy_users")]
    public class WdsUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("clonedeploy_user_id", Order = 1)]
        public int Id { get; set; }

        [Column("clonedeploy_username", Order = 2)]
        public string Name { get; set; }

        [Column("clonedeploy_user_pwd", Order = 3)]
        public string Password { get; set; }

        [Column("clonedeploy_user_salt", Order = 4)]
        public string Salt { get; set; }

        [Column("clonedeploy_user_role", Order = 5)]
        public string Membership { get; set; }

        [NotMapped]
        public string GroupManagement { get; set; }

        [NotMapped]
        public string OndAccess { get; set; }

        [NotMapped]
        public string DebugAccess { get; set; }

        [NotMapped]
        public string DiagAccess { get; set; }
        
        
    }
}