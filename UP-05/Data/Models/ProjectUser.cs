namespace UP_05.Data.Models
{
    public class ProjectUser
    {
        private int id { get; set; }
        private Project projectId { get; set; }
        private User userId { get; set; }
        public enum role
        {
            creator,
            editor,
            reader
        }
        public ProjectUser(int id, Project projectId, User userId, role role) 
        {
            this.id = id;
            this.projectId = projectId;
            this.userId = userId;
        }
    }
}
