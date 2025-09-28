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
        public Guid Id { get; set; }

        [Required]
        public Guid Message_id { get; set; }

        [Required]
        [ForeignKey(nameof(Message_id))]
        public virtual Message Message { get; set; }

        [Required]
        public Guid Task_id { get; set; }

        [Required]
        [ForeignKey(nameof(Task_id))]
        public virtual Task Task { get; set; }
    }
}
