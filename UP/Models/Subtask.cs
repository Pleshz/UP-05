using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Subtask(string Name, string Description, int TaskId, int AssignedUserId, int ColumnId, DateTime DueDate, DateTime CreatedAt, DateTime UpdatedAt)
        {
            this.Name = Name;
            this.Description = Description;
            this.TaskId = TaskId;
            this.AssignedUserId = AssignedUserId;
            this.ColumnId = ColumnId;
            this.DueDate = DueDate;
            this.CreatedAt = CreatedAt;
            this.UpdatedAt = UpdatedAt;
        }

        public Subtask(int Id, string Name, string Description, int TaskId, int AssignedUserId, int ColumnId, DateTime DueDate, DateTime CreatedAt, DateTime UpdatedAt)
        {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.TaskId = TaskId;
            this.AssignedUserId = AssignedUserId;
            this.ColumnId = ColumnId;
            this.DueDate = DueDate;
            this.CreatedAt = CreatedAt;
            this.UpdatedAt = UpdatedAt;
        }
    }
}
