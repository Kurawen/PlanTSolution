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
    public class Task_comment // под большим вопросом существования

    {
        public Guid Id { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [Required]
        [ForeignKey(nameof(User_id))]
        public virtual User User { get; set; }

        [Required]
        [MaxLength(300)]
        public string Content { get; set; }

        [Required]
        public DateTime Created_at { get; set; }

        [Required]
        public Guid Task_id { get; set; }

        [Required]  
        [ForeignKey(nameof(Task_id))]
        public virtual Task Task { get; set; }

    }
}
