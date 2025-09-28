using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Tools;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Entities
{
    public class Group_members
    {
        [Key]
        public Guid GroupMemberId { get; set; }

        [Required]
        public int User_id { get; set; }

        [Required]
        [ForeignKey("User_id")]
        public virtual Users User  { get; set; }

        [Required]
        public int Group_id { get; set; }

        [Required]
        [ForeignKey("Group_id")]
        public virtual Groups Group { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public DateTime Joined_at { get; set; }

    }
}
