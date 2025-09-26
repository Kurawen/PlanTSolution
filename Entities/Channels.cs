using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations

namespace Entities
{
    public class Channels
    {
        public Guid Id { get; set; }

        [ForeignKey("GroupsId")]
        public virtual Groups Group {  get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public Boolean IsPrivate { get; set; }

        
    }
}
