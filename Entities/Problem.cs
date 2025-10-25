using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Problem
    {
        public Guid Id { get; set; }

        [Required]
        public Guid Created_by { get; set; } // ID пользователя, который создал задачу

        [Required]
        public Guid Assigned_to { get; set; } // ID пользователя, которому назначена задача

        [Required]
        public Guid Group_id { get; set; } // ID группы, к которой относится задача

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, in_progress, completed, cancelled

        [Required]
        [MaxLength(10)]
        public string Priority { get; set; } = "medium"; // low, medium, high, urgent

        [Required]
        public DateTime Due_date { get; set; }

        [Required]
        public DateTime Created_at { get; set; }

        [Required]
        public DateTime Updated_at { get; set; }
    }
}