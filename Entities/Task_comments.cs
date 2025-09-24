using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    internal class Task_comments

    {
        public Guid Id { get; set; }

        [ForeignKey("UsersID")]
        public virtual Users User { get; set; }

        [MaxLength(300)]
        public string Content { get; set; }

        public DateTime Created_at { get; set; }

        [ForeignKey("TasksId")]
        public virtual Tasks Task { get; set; }

    }
}
