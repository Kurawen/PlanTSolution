using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Channels
    {
        public Guid Id { get; set; }

        [ForeignKey("GroupsId")]
        public virtual Groups Group {  get; set; }

        public string Name { get; set; }

        public Boolean IsPrivate { get; set; }

        
    }
}
