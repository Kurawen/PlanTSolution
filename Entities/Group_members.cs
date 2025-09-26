using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations

namespace Entities
{
    public class Group_members
    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("UsersId")]
        public virtual Users User  { get; set; }

        [Required]
        [ForeignKey("GrupsId")]
        public virtual Groups Group {  get; set; }

        [Required]
        public string Role {  get; set; }
        [Required]
        public DateTime Joined_at { get; set; }

    }
}
