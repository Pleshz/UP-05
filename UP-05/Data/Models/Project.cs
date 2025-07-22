namespace UP_05.Data.Models
{
    public class Project
    {
        private int id { get; set; }
        private string name { get; set; }
        private string description { get; set; }
        private bool isPublic { get; set; }
        public Project(Project project = null) 
        {
            if (project != null) 
            {
                this.id = project.id;
                this.name = project.name;
                this.description = project.description;
                this.isPublic = project.isPublic;
            }
        }
        public Project() { }
    }
}
