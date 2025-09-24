using System.ComponentModel.DataAnnotations;

namespace Entities
{   //Задачи

    public class Tasks
    {
        public Guid Id { get; set; }
        public int Created_by { get; set; }
        public int Assignet_by { get; set; }
        public int Group_id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [MaxLength(10)]
        public string Status { get; set; }
        [MaxLength(10)]
        public string Priority { get; set; }
        
        public DateTime Due_date { get; set; }

        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set;


    }
}
