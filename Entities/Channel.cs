using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.EntityFrameworkCore.Tools;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Entities
{
    public class Channel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid Group_id { get; set; }

        [ForeignKey(nameof(Group_id))]
        public virtual Group Group { get; set; }

        [MaxLength(36)]
        [Required]
        public string Name { get; set; }
        
        [Required]
        public Boolean IsPrivate { get; set; }

        
    }
}
