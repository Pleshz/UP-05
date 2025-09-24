using System;

namespace UP.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ColumnId { get; set; }
        public int ProjectId { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
