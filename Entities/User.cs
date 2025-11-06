using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class User
    {
        public Guid Id { get; set; }

        [MaxLength(36)]
        
        public string First_Name { get; set; }

        [MaxLength(36)]
        
        public string Last_Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime Created_at { get; set; }
    }
}