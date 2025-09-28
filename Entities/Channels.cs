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
    public class Channels
    {
        [Key]
        public Guid ChannelId { get; set; }

        [Required]
        public int Group_id { get; set; }

        [ForeignKey("Group_id")]
        public virtual Groups Group { get; set; }

        [Required]
        public string Name { get; set; }
        
        [Required]
        public Boolean IsPrivate { get; set; }

        
    }
}
