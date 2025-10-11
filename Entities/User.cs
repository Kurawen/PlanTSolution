using System;
using System.Collections.Generic;
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
    public class User
    {
        public Guid Id { get; set; }

        [MaxLength(36)]
        [Required]
        public string First_Name { get; set; }

        [MaxLength(36)]
        [Required]
        public string Last_Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public DateTime Created_at { get; set; }
    }
}
