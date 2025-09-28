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
    public class Messages
    {
        [Key] // не будет лишним указать, что это первичный ключ
        public Guid MessageId { get; set; }

        [Required] // навигационное свойство
        public int User_id { get; set; }

        [Required] // сам внешний ключ
        [ForeignKey("User_id")]
        public virtual Users User { get; set; }

        [Required] // навигационное свойство
        public int Channel_id { get; set; }

        [Required]
        [ForeignKey("Channel_id")] // сам внешний ключ
        public virtual Channels Channel { get; set; }

        [Required]
        public string Content { get; set; } 
        
        [Required]
        public DateTime Sent_at { get; set; }
    }
}
