using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Groups
    {
        public Guid Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public DateTime Created_at { get; set; }

        public Boolean Is_public { get; set; }

    }
}
