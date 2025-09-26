using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations
namespace Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("MessagesId")]
        public virtual Messages Messages { get; set; }

        [Required]
        [ForeignKey("TasksId")]
        public virtual Tasks Tasks { get; set; }
    }
}
