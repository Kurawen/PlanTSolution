using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Message
    {
        public Guid Id { get; set; }

        [Required]
        public Guid User_id { get; set; }

        [Required]
        public Guid Channel_id { get; set; }

        [MaxLength(1000)]
        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Sent_at { get; set; }
    }
}