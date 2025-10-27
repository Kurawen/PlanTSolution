using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Notification
    {
        public Guid Id { get; set; }

        [Required]
        public Guid User_id { get; set; } // Добавил User_id - кому предназначено уведомление

        [Required]
        public Guid Message_id { get; set; }

        [Required]
        public Guid Problem_id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Type { get; set; } // "message", "task_assigned", "task_completed" и т.д.

        [MaxLength(500)]
        public string Content { get; set; }

        [Required]
        public bool Is_read { get; set; } = false;

        [Required]
        public DateTime Created_at { get; set; }
    }
}