using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Users_profiels

    {
        [Required]
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        
        [Required]
        public string Bio {  get; set; }

        [Required]
        public string Phone_number { get; set; }


        public string Avatar_url { get; set; }


    }
}
