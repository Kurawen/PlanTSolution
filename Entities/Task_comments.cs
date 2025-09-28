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
    public class Task_comments // под большим вопросом существования

    {
        [key]
        public Guid TaskCommentId { get; set; }

        [Required]
        public int User_id { get; set; }

        [Required]
        [ForeignKey("User_id")]
        public virtual Users User { get; set; }

        [Required]
        [MaxLength(300)]
        public string Content { get; set; }

        [Required]
        public DateTime Created_at { get; set; }

        [Required]
        public int Task_id { get; set; }

        [Required]  
        [ForeignKey("Task_id")]
        public virtual Tasks Task { get; set; }

    }
}
