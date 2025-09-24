using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Messages
    {
        public Guid Id { get; set; }

        [ForeignKey("UsersId")]
        public virtual Users Users { get; set; }

        [ForeignKey("ChanelsId")]
        public virtual Channels Channels { get; set; }  

        public string Content { get; set; } 

        public DateTime Sent_at { get; set; }
    }
}
