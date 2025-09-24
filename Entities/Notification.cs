using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        [ForeignKey("MessagesId")]
        public virtual Messages Messages { get; set; }

        [ForeignKey("TasksId")]
        public virtual Tasks Tasks { get; set; }
    }
}
