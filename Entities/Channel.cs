using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Channel
    {
        public Guid Id { get; set; }

        [Required]
        public Guid Group_id { get; set; }

        [MaxLength(36)]
        [Required]
        public string Name { get; set; }

        [Required]
        public Boolean IsPrivate { get; set; }
    }
}