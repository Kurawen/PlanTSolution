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
    public class Group_member
    {
        public Guid Id { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [Required]
        [ForeignKey(nameof(User_id))]
        public virtual User User  { get; set; }

        [Required]
        public Guid Group_id { get; set; }

        [Required]
        [ForeignKey(nameof(Group_id))]
        public virtual Group Group { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public DateTime Joined_at { get; set; }

    }
}
