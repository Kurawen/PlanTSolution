using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User_password
    {
        public Guid Id { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [Required]
        public string Hash_password { get; set; }

        [Required]
        public DateTime Created_at { get; set; }

        public DateTime? Updated_at { get; set; }
    }
}