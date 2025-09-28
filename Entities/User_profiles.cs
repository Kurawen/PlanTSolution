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
    public class Users_profiels

    {
        [Key]
        public Guid UserProfileId { get; set; }

        [Required]
        public int User_id { get; set; }

        [Required]
        [ForeignKey("User_id")]
        public virtual Users User { get; set; }
        
        [Required] // под большим вопросом существования
        public string Bio {  get; set; }

        [Required]
        public string Phone_number { get; set; }


        public string Avatar_url { get; set; }
    }
}
