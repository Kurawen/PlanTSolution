using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.EntityFrameworkCore.Tools;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Entities
{
    public class User_profile

    {
        public Guid Id { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [Required]
        [ForeignKey(nameof(User_id))]
        public virtual User User { get; set; }
        
        [Required] // под большим вопросом существования
        public string Bio {  get; set; }

        [Required]
        public string Phone_number { get; set; }


        public string Avatar_url { get; set; }
    }
}
