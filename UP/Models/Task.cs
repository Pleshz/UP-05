using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task(string Title, string Description, int ColumnId, int ProjectId, int CreatorId, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DueDate = null)
        {
            this.Title = Title;
            this.Description = Description;
            this.ColumnId = ColumnId;
            this.ProjectId = ProjectId;
            this.CreatorId = CreatorId;
            this.CreatedAt = CreatedAt;
            this.UpdatedAt = UpdatedAt;
            this.DueDate = DueDate;
        }

        public Task(int Id, string Title, string Description, int ColumnId, int ProjectId, int CreatorId, DateTime CreatedAt, DateTime UpdatedAt, DateTime? DueDate) {
            this.Id = Id;
            this.Title = Title;
            this.Description = Description;
            this.ColumnId = ColumnId;
            this.ProjectId = ProjectId;
            this.CreatorId = CreatorId;
            this.CreatedAt = CreatedAt;
            this.UpdatedAt = UpdatedAt;
            this.DueDate = DueDate;
        }
    }
}
