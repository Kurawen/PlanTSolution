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
    public class Message
    {
        public Guid Id { get; set; }

        [Required] // навигационное свойство
        public Guid User_id { get; set; }

        [Required] // сам внешний ключ
        [ForeignKey(nameof(User_id))]
        public virtual User User { get; set; }

        [Required] // навигационное свойство
        public Guid Channel_id { get; set; }

        [Required]
        [ForeignKey(nameof(Channel_id))] // сам внешний ключ
        public virtual Channel Channel { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Content { get; set; } 
        
        [Required]
        public DateTime Sent_at { get; set; }
    }
}
