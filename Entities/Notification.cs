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
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }

        [Required]
        public int Message_id { get; set; }

        [Required]
        [ForeignKey("Message_id")]
        public virtual Messages Message { get; set; }

        [Required]
        public int Task_id { get; set; }

        [Required]
        [ForeignKey("Task_id")]
        public virtual Tasks Task { get; set; }
    }
}
