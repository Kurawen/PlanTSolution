using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Tools;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Entities
{   //Задачи

    public class Tasks
    {
        public Guid Id { get; set; }

        [Required]
        public int Created_by { get; set; }

        // Это внешний ключ?
        //[Required]
        //public int Assignet_by { get; set; }

        // Это внешний ключ?
        [Required]
        [ForeignKey("GroupId")]
        public virtual Groups Groups { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(10)]
        public string Status { get; set; }

        [Required]
        [MaxLength(10)]
        public string Priority { get; set; }

        [Required]
        public DateTime Due_date { get; set; }

        [Required]
        public DateTime Created_at { get; set; }

        [Required]
        public DateTime Updated_at { get; set; }
    }
}
