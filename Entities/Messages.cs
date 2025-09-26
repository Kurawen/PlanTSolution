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
    public class Messages
    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("UsersId")]
        public virtual Users Users { get; set; }

        [Required]
        [ForeignKey("ChanelsId")]
        public virtual Channels Channels { get; set; }  

        [Required]
        public string Content { get; set; } 
        
        [Required]
        public DateTime Sent_at { get; set; }
    }
}
