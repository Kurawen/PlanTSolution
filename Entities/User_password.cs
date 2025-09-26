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
    public class User_password
    {
        public Guid Id { get; set; }

        [Required]
        [ForeignKey("UsersId")]
        public virtual Users User { get; set; }

        [Required]
        public string Hash_password { get; set; }
    }
}
