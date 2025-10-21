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
    public class User_profile
    {
        public Guid Id { get; set; }

        [Required]
        public Guid User_id { get; set; }

        public string Phone_number { get; set; }

        public string Avatar_url { get; set; }
    }
}