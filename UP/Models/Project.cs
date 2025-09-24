using System;

namespace UP.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isPublic { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public Project(string Name, string Description, bool isPublic, int CreatorId, DateTime CreatedAt, DateTime UpdateAt)
        {
            this.Name = Name;
            this.Description = Description;
            this.isPublic = isPublic;
            this.CreatorId = CreatorId;
            this.CreatedAt = CreatedAt;
            this.UpdateAt = UpdateAt;
        }

        public Project(int Id, string Name, string Description, bool isPublic, int CreatorId, DateTime CreatedAt, DateTime UpdateAt) {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.isPublic = isPublic;
            this.CreatorId = CreatorId;
            this.CreatedAt = CreatedAt;
            this.UpdateAt = UpdateAt;
        }
    }
}
