using System;

namespace UP.Models
{
    public class Subtask
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskId { get; set; }
        public int AssignedUserId { get; set; }
        public int ColumnId { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
