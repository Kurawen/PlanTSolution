using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public string First_Name { get; set; }

        public string Last_Name { get; set;


        public string Email { get; set; }
        
        public DateTime Created_at { get; set; }
    }
}
