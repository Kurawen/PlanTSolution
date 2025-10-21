using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Group_member
    {
        public Guid Id { get; set; }

        [Required]
        public Guid Group_id { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [Required]
        public string Role { get; set; } = "member"; // member, admin, owner

        [Required]
        public DateTime Joined_at { get; set; }
    }
}