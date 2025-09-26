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
    internal class Task_comments

    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("UsersID")]
        public virtual Users User { get; set; }

        [Required]
        [MaxLength(300)]
        public string Content { get; set; }

        [Required]
        public DateTime Created_at { get; set; }

        [Required]  
        [ForeignKey("TasksId")]
        public virtual Tasks Task { get; set; }

    }
}
