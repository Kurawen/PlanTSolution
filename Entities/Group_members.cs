using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Group_members
    {
        public Guid Id { get; set; }

        [ForeignKey("UsersId")]
        public virtual Users User  { get; set; }

        [ForeignKey("GrupsId")]
        public virtual Groups Group {  get; set; }

        public string Role {  get; set; }

        public DateTime Joined_at { get; set; }

    }
}
